using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace WpfLINQtoSQLCompanyBenefit
{
    /// <summary>
    /// Interaction logic for AddPersonDialog.xaml
    /// </summary>
    public partial class AddPersonDialog : Window
    {
        LINQtoSQLdataClassesDataContext dataContext;

        public AddPersonDialog()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WpfLINQtoSQLCompanyBenefit.Properties.Settings.CompanyBenefitDBConnectionString"].ConnectionString;

            dataContext = new LINQtoSQLdataClassesDataContext(connectionString);
        }

        public Person Person { get; set; }

        private void AddPersonButton(object sender, RoutedEventArgs e)
        {
            Person = new Person()
            {
                PersonName = TxtName.Text,
                Gender = TxtGender.Text,
                PostId = int.Parse(myCombo.Text)
            };

            dataContext.Persons.InsertOnSubmit(Person);
            dataContext.SubmitChanges();
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var postIds = dataContext.Posts.Select(x => x.Id);

            foreach(var postId in postIds)
            {
                myCombo.Items.Add(postId);
            }
        }
    }
}
