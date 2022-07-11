﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSUtility.Domain.AppModels;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices.AdapterProperties;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    private NameserverViewModel? _selectedNameserver;

    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark,
        MainWindowViewModel mainWindowViewModel)
    {
        // Get access to main view model instance
        MainViewModel = mainWindowViewModel;

        // Initialize the list of nameservers for the UI
        Nameservers = new ObservableCollection<NameserverViewModel>();

        //nameservers = nameservers.Take(10);

        // Convert list of nameservers to a list of NameserverViewModels
        foreach (var nameserver in nameservers)
        {
            var nameserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(nameserverViewModel);
        }

        BenchmarkTasks = new List<Task>();


        RunDnsTest = ReactiveCommand.Create(() =>
        {
            foreach (var nameserver in Nameservers) BenchmarkTasks.Add(BenchmarkNameserver(nameserver));


            // Wait for all benchmarks to be completed
            Task.WaitAll(BenchmarkTasks.ToArray());
        });

        // TODO: Merge the following TWO commands into ONE function with different parameters 
        // Command for applying primary dns server
        ApplyPrimary = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ApplyPrimary(SelectedNameserver.IpAddress,
                            MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });
        // Command for applying secondary dns server
        ApplySecondary = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ApplySecondary(SelectedNameserver.IpAddress,
                            MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });

        this.WhenAnyValue(x => x.SelectedNameserver)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(CreatePlot);
    }

    public List<Task> BenchmarkTasks { get; set; }

    public ObservableCollection<NameserverViewModel> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    public ReactiveCommand<Unit, Unit> ApplyPrimary { get; set; }

    public ReactiveCommand<Unit, Unit> ApplySecondary { get; set; }


    // The minimum average ping found in the nameserver list. Used to determine the status of the nameserver
    public int MinPingAverage { get; set; }

    // The maximum average ping found in the nameserver list. Used to determine the status of the nameserver
    public int MaxPingAverage { get; set; }

    public NameserverViewModel? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    public MainWindowViewModel MainViewModel { get; set; }

    private Task BenchmarkNameserver(NameserverViewModel nameserver)
    {
        return Task.Run(() =>
        {
            for (var j = 0; j < 5; j++)
            {
                SendPing(nameserver);
                /*var reply = pingBenchmark.Run(nameserver.IpAddress);

                // Set the reply as the latest ping
                nameserver.LatestPing = (ushort)reply.RoundtripTime;

                // Add the reply to the list of pings
                nameserver.Pings.Add((ushort)reply.RoundtripTime);

                // Calculate the average ping so it can be displayed in the view
                nameserver.CalculateAveragePing();

                // If this test resulted in a ping of 0, return and free the task
                if (nameserver.AveragePing == 0) return Task.CompletedTask;*/

                // Calculate the average ping so it can be displayed in the view
                nameserver.CalculateAveragePing();
            }

            return Task.CompletedTask;
        });
    }

    // TODO Move to services project
    private void SendPing(NameserverViewModel nameserver)
    {
        // Create ping sender
        var pingSender = new Ping();

        // Define the callback object
        pingSender.PingCompleted += PingCompletedCallback;

        // Create the data to send
        var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var buffer = Encoding.ASCII.GetBytes(data);

        // Ping the server
        pingSender.SendAsync(nameserver.IpAddress, 10000, buffer, nameserver);
    }

    // TODO Move to services project
    private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
    {
        var nameserver = (NameserverViewModel)e.UserState!;

        var reply = e.Reply;

        // Set the reply as the latest ping
        if (reply != null)
        {
            nameserver.LatestPing = (ushort)reply.RoundtripTime;

            // Add the reply to the list of pings
            nameserver.Pings.Add((ushort)reply.RoundtripTime);
        }
    }

    /// <summary>
    ///     A method to remove the poor quality nameservers.
    /// </summary>
    private void RemovePoorQualityNameservers()
    {
        for (var i = 0; i < Nameservers.Count; i++)
            if (Nameservers[i].LatestPing == 0)
                Nameservers.RemoveAt(i);
    }

    private void RunTests(IBenchmark pingBenchmark, NameserverViewModel nameserver)
    {
    }

    private async void CreatePlot(NameserverViewModel? nameserver)
    {
        if (nameserver == null)
        {
            return;
        }

        MainViewModel.GraphViewModel = new GraphViewModel(nameserver);
    }
}