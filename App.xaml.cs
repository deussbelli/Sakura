using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Mathematical_cryptology_RailFenceCipher;



namespace Mathematical_cryptology
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RailFenceCipher railFenceCipher = new RailFenceCipher();
            railFenceCipher.Show();

        }
    }

}