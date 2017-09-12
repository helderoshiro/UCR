﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UCR.Annotations;
using UCR.Core.Device;

namespace UCR.ViewModels.Device
{
    public class DeviceGroupViewModel : INotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get { return this.title; }
            set
            {
                if (value == this.title) return;
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public Guid Guid { get; set; }
        public ObservableCollection<DeviceGroupViewModel> Groups { get; set; }
        public ObservableCollection<Core.Device.Device> Devices { get; set; }

        public DeviceGroupViewModel(string title = null, Guid guid = new Guid())
        {
            Title = title;
            Groups = new ObservableCollection<DeviceGroupViewModel>();
            Devices = new ObservableCollection<Core.Device.Device>();
            Guid = guid.Equals(Guid.Empty) ? Guid.NewGuid() : guid;
        }

        public DeviceGroupViewModel(DeviceGroup deviceGroup) : this(deviceGroup.Title, deviceGroup.Guid)
        {
            deviceGroup.Devices.ForEach(d => Devices.Add(d));
        }

        public IList Items
        {
            get
            {
                var items = new CompositeCollection
                {
                    new CollectionContainer {Collection = Groups},
                    new CollectionContainer {Collection = Devices}
                };
                return items;
            }
        }

        public static DeviceGroupViewModel FindDeviceGroupViewModelWithDevice(Collection<DeviceGroupViewModel> deviceGroupViewModels, Core.Device.Device device)
        {
            DeviceGroupViewModel result = null;
            foreach (var deviceGroupViewModel in deviceGroupViewModels)
            {
                if (deviceGroupViewModel.Devices.Contains(device)) result = deviceGroupViewModel;
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
