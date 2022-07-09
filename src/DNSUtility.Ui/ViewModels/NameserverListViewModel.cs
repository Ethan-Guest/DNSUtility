﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
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

        // Convert list of nameservers to a list of NameserverViewModels
        foreach (var nameserver in nameservers)
        {
            var nameserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(nameserverViewModel);
        }

        RunDnsTest = ReactiveCommand.Create(
            () =>
            {
                foreach (var nameserver in Nameservers)
                    // Create a new thread for each test
                    new Thread(() =>
                    {
                        // Run the test 5 times
                        for (int i = 0; i < 5; i++)
                        {
                            ushort ping = pingBenchmark.RunBenchmark(nameserver.IpAddress);


                            // Set the reply as the latest ping
                            nameserver.LatestPing = ping;

                            // Add the reply to the list of pings
                            nameserver.Pings.Add(ping);

                            // If the ping replies with 0, remove it from the list
                            /*if (ping == 0)
                            {
                                Nameservers.Remove(nameserver);
                                break;
                            }*/
                        }
                    }).Start();
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

    public ObservableCollection<NameserverViewModel> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    public ReactiveCommand<Unit, Unit> ApplyPrimary { get; set; }

    public ReactiveCommand<Unit, Unit> ApplySecondary { get; set; }


    public NameserverViewModel? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    public MainWindowViewModel MainViewModel { get; set; }

    private async void CreatePlot(NameserverViewModel? nameserver)
    {
        if (nameserver == null)
        {
            return;
        }

        MainViewModel.GraphViewModel = new GraphViewModel(nameserver);
    }
}