using System.ComponentModel;
using Xamarin.Forms;

namespace FormsControlExample
{
    public class ExampleViewModel : INotifyPropertyChanged
    {
        string statusMessage = "";
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                if (statusMessage != value)
                {
                    statusMessage = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs("StatusMessage"));
                }
            }
        }

        public Command MyCommand { get; private set; }

        public ExampleViewModel()
        {
            MyCommand = new Command(() =>
            {
                StatusMessage = "You have activated the command!";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
