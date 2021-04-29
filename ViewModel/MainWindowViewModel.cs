using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfTestApp.Commands;

namespace WpfTestApp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        PersonInfoServiceReference.Service1Client personInfoServiceReference = new PersonInfoServiceReference.Service1Client();
        private ICommand getCodeExecuteCommand { get; set; }
        public ICommand GetCodeExecuteCommand
        {
            get
            {
                return getCodeExecuteCommand;
            }
            set
            {
                getCodeExecuteCommand = value;
            }
        }
        private int recordCount = 0;
        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                if (recordCount != value)
                {
                    SetPropertry(ref this.recordCount, value);
                }
            }
        }

        private int totalRecord = 0;
        public int TotalRecord
        {
            get { return totalRecord; }
            set
            {
                if (totalRecord != value)
                {
                    SetPropertry(ref this.totalRecord, value);
                }
            }
        }

        private ObservableCollection<Model.PersonInfo> testDataCollection;

        public ObservableCollection<Model.PersonInfo> TestDataCollection
        {
            get { return testDataCollection; }
            set
            {
                SetPropertry(ref testDataCollection, value);
            }
        }

        public MainWindowViewModel()
        {
            TestDataCollection = new ObservableCollection<Model.PersonInfo>();
            GetCodeExecuteCommand = new RelayCommand(GetDataExecute, CanGetDataExecute);
        }

        private bool CanGetDataExecute(object obj)
        {
            return RecordCount == 0 || RecordCount == TotalRecord;
        }

        private void GetDataExecute(object obj)
        {
            GetAndDisplayTestData();
        }

        private void GetAndDisplayTestData()
        {
            try
            {
                RecordCount = 0;
                var mydt = personInfoServiceReference.GetPesrsonInfoData();

                if (mydt.Length > 0)
                {
                    TotalRecord = mydt.Length;
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Tick += (s, ea) =>
                    {
                        RecordCount += 1;
                        foreach (var infoItem in mydt)
                        {
                            if (TestDataCollection.AsEnumerable().Where(x=> x.Email == infoItem.Email).Count() == 0 )
                            {
                                Model.PersonInfo mydata = new Model.PersonInfo();
                                mydata.FirstName = infoItem.FirstName;
                                mydata.LastName = infoItem.LastName;
                                mydata.Email = infoItem.Email;
                                mydata.Gender = infoItem.Gender;

                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    TestDataCollection.Add(mydata);
                                });
                                break;
                            }
                        }
                        if (RecordCount >= TotalRecord)
                        {
                            timer.Stop();
                        }
                    };
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 600);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
