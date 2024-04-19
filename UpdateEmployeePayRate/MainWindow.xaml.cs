
/* Title"           Update Employee Labor Rate
 * Date:            11-29-18
 * Author:          Terry Holmes
 * 
 * Description:     This application will update employee pay rate */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EmployeeLaborRateDLL;
using NewEventLogDLL;
using NewEmployeeDLL;

namespace UpdateEmployeePayRate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting the classes up
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeLaborRateClass TheEmployeeLaborRateClass = new EmployeeLaborRateClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();

        EmployeesDataSet TheEmployeesDataSet;
        FindEmployeeLaborRateDataSet TheFindEmployeeLaborRateDataSet = new FindEmployeeLaborRateDataSet();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheEmployeesDataSet = TheEmployeeClass.GetEmployeesInfo();

            dgrResults.ItemsSource = TheEmployeesDataSet.employees;
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;
            int intRecordsReturned;
            int intEmployeeID;
            decimal decPayRate = 1;
            bool blnFatalError = false;

            try
            {
                intNumberOfRecords = TheEmployeesDataSet.employees.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    intEmployeeID = TheEmployeesDataSet.employees[intCounter].EmployeeID;

                    TheFindEmployeeLaborRateDataSet = TheEmployeeLaborRateClass.FindEmployeeLaborRate(intEmployeeID);

                    intRecordsReturned = TheFindEmployeeLaborRateDataSet.FindEmployeeLaborRate.Rows.Count;

                    if(intRecordsReturned < 1)
                    {
                        blnFatalError = TheEmployeeLaborRateClass.InsertEmployeeLaborRate(intEmployeeID, decPayRate);

                        if (blnFatalError == true)
                            throw new Exception();
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update Employee Pay Rate // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
