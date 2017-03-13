﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace Waf.Writer.Applications.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : Model, IShellService
    {
        private object shellView;
        private string documentName;
        private IEditingCommands activeEditingCommands;
        private IZoomCommands activeZoomCommands;


        [ImportingConstructor]
        public ShellService()
        {
            activeEditingCommands = new DisabledEditingCommands();
            activeZoomCommands = new DisabledZoomCommands();
        }


        public object ShellView
        {
            get { return shellView; }
            set { SetProperty(ref shellView, value); }
        }

        public string DocumentName
        {
            get { return documentName; }
            set { SetProperty(ref documentName, value); }
        }

        public IEditingCommands ActiveEditingCommands
        {
            get { return activeEditingCommands; }
            set { SetProperty(ref activeEditingCommands, value ?? new DisabledEditingCommands()); }
        }

        public IZoomCommands ActiveZoomCommands
        {
            get { return activeZoomCommands; }
            set { SetProperty(ref activeZoomCommands, value ?? new DisabledZoomCommands()); }
        }


        private class DisabledEditingCommands : Model, IEditingCommands
        {
            public bool IsBold { get; set; }
            
            public bool IsItalic { get; set; }
            
            public bool IsUnderline { get; set; }
            
            public bool IsNumberedList { get; set; }
            
            public bool IsBulletList { get; set; }

            public bool IsSpellCheckAvailable { get; } = false;

            public bool IsSpellCheckEnabled { get; set; }
        }

        private class DisabledZoomCommands : Model, IZoomCommands
        {
            private readonly DelegateCommand disabledCommand = new DelegateCommand(() => { }, () => false);


            public IReadOnlyList<string> DefaultZooms { get; } = null;

            public double Zoom
            {
                get { return 1; }
                set { }
            }
            
            public ICommand ZoomInCommand => disabledCommand;
            
            public ICommand ZoomOutCommand => disabledCommand;
            
            public ICommand FitToWidthCommand => disabledCommand;
        }
    }
}
