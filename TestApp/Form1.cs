using Muses.Slf;
using Muses.Slf.Interfaces;
using System;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        ILoggerFactory _factory;
        ILogger _logger;

        public Form1()
        {
            InitializeComponent();

            var fa = ConcreteLoader.LoadFactories();
            foreach(var f in fa)
            {
                factories.Items.Add(f);
            }
        }

        void log(Level level, String message)
        {
            if(_logger != null)
            {
                _logger.Log(level, null, message);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            log(Level.Trace, "Hello World!");
            log(Level.Debug, "Hello World!");
            log(Level.Info, "Hello World!");
            log(Level.Warning, "Hello World!");
            log(Level.Error, "Hello World!");
            log(Level.Fatal, "Hello World!");
        }

        void Listen(LogEvent ev)
        {
            logBox.Text = ev.RenderedMessage + Environment.NewLine + logBox.Text;
        }

        private void factories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_factory != null)
            {
                _factory.UnregisterEventListener(Listen);
            }

            var factory = factories.SelectedItem as ILoggerFactory;
            if(factory != null)
            {
                factory.RegisterEventListener(Listen);
                _factory = factory;
                _logger = factory.GetLogger(typeof(Form1));
                _logger.Info($"Started {factory.Name} logger...");
            }
        }
    }
}
