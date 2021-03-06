﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/8/22 1:45:06 (UTC +8:00)
** desc： ...
*************************************************/
using AutumnBox.Basic.Calling.Adb;
using AutumnBox.Basic.Device;
using AutumnBox.Basic.Util;
using AutumnBox.GUI.MVVM;
using AutumnBox.GUI.Util.Bus;
using AutumnBox.GUI.Util.Debugging;
using AutumnBox.GUI.View.Windows;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AutumnBox.GUI.ViewModel
{
    class VMEnableDeviceNetDebugging : ViewModelBase
    {
        private const string PORT_HINT_KEY = "ContentEnableDeviceNetDebuggingPortHint";
        private const string PORT_ERR_HINT_KEY = "ContentEnableDeviceNetDebuggingPortErrorHint";
        public Action ViewCloser { get; set; }
        #region MVVM
        public string Hint
        {
            get => _hint; set
            {
                _hint = value;
                RaisePropertyChanged();
            }
        }
        private string _hint;
        public string PortString
        {
            get => _portString; set
            {
                _portString = value;
                RaisePropertyChanged();
            }
        }
        private string _portString = "5555";
        public FlexiableCommand Open { get; private set; }
        #endregion
        private readonly ILogger logger;
        public VMEnableDeviceNetDebugging()
        {
            logger = new Logger<VMEnableDeviceNetDebugging>();
            Open = new FlexiableCommand(OpenImpl);
            Hint = App.Current.Resources[PORT_HINT_KEY].ToString();
        }
        private void OpenImpl()
        {
            Task.Run(() =>
            {
                try
                {
                    UsbDevice target = (UsbDevice)DeviceSelectionObserver.Instance.CurrentDevice;
                    ushort port = 0;
                    try
                    {
                        port = ushort.Parse(PortString);
                    }
                    catch
                    {
                        Hint = App.Current.Resources[PORT_ERR_HINT_KEY].ToString();
                        return;
                    }
                    var ip = target.GetLanIP();
                    target.OpenNetDebugging(port);
                    if (ip == null)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            new MessageWindow()
                            {
                                MsgTitle = "Warning",
                                Message = "ContentEnableDeviceNetDebuggingGetIPFailed",
                            }.Show();
                        });
                    }
                    else
                    {
                        new AdbCommand($"connect {ip}:{port}").To((e) =>
                        {
                            logger.Info(e.Text);
                        }).Execute();
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("connect failed", ex);
                }
            });
            ViewCloser();
        }
        private void ConnectTo(IPEndPoint endPoint)
        {
            try
            {
                new AdbCommand($"{endPoint.Address}:{endPoint.Port}").To((e) =>
                {
                    logger.Info(e.Text);
                }).Execute().ThrowIfExitCodeNotEqualsZero();
            }
            catch (Exception ex)
            {
                logger.Warn("a exception happend on connect to new device", ex);
            }
        }
    }
}
