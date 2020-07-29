using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace ArduinoMornitoring.Views
{
    /// <summary>
    /// ShellView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            Graph_Sample();
            //InitControls();
        }

        private void Graph_Sample()
        {
            var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
            var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();

            linegraph.Plot(x, y);
        }
        
        
    }
}
