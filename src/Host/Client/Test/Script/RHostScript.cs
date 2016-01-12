﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.R.Support.Settings;

namespace Microsoft.R.Host.Client.Test.Script {
    [ExcludeFromCodeCoverage]
    public class RHostScript : IDisposable {
        public IRSessionProvider SessionProvider { get; private set; }
        public IRSession Session { get; private set; }

        public RHostScript(IRSessionProvider sessionProvider) {
            SessionProvider = sessionProvider;
            Session = SessionProvider.Create(0, new RHostClientTestApp());
            Session.StartHostAsync(new RHostStartupInfo {
                Name = "RHostScript",
                RBasePath = RToolsSettings.Current.RBasePath,
                RCommandLineArguments = RToolsSettings.Current.RCommandLineArguments,
                CranMirrorName = RToolsSettings.Current.CranMirror
            }).Wait();
        }

        public void Dispose() {
            if (Session != null) {
                Session.StopHostAsync().Wait();
                Session.Dispose();
                Session = null;
            }

            if(SessionProvider != null) {
                SessionProvider.Dispose();
                SessionProvider = null;
            }
        }
    }
}