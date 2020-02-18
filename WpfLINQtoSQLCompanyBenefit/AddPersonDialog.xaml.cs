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
using System.Windows.Shapes;

namespace WpfLINQtoSQLCompanyBenefit
{
    /// <summary>
    /// Interaction logic for AddPersonDialog.xaml
    /// </summary>
    public partial class AddPersonDialog : Window
    {
        public AddPersonDialog()
        {
            InitializeComponent();
        }

        public Person Person { get; set; }

        private void AddPersonButton(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Add clicked!!");
            Person = new Person()
            {
                PersonName = TxtName.Text,
                Gender = TxtGender.Text,
                //PostId = int.Parse(TxtPost.Text)
            };
            DialogResult = true;
        }
    }
}
