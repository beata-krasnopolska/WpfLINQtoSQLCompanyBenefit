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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfLINQtoSQLCompanyBenefit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LINQtoSQLdataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WpfLINQtoSQLCompanyBenefit.Properties.Settings.CompanyBenefitDBConnectionString"].ConnectionString;

            dataContext = new LINQtoSQLdataClassesDataContext(connectionString);
        }

        public void InsertPost()
        {
            var manager = dataContext.Posts.SingleOrDefault(x => x.PostName == "Manager");

            if (manager == null)
            {
                Post Manager = new Post();
                Manager.PostName = "Manager";
                dataContext.Posts.InsertOnSubmit(Manager);
            }

            var assistant = dataContext.Posts.SingleOrDefault(x => x.PostName == "Assistant");

            if (assistant == null)
            {
                Post Assistant = new Post();
                Assistant.PostName = "Assistant";
                dataContext.Posts.InsertOnSubmit(Assistant);
            }

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Posts.Select(x => new Entity.Post
            {
                Id = x.Id,
                PostName = x.PostName
            }).ToList();
        }

        public void InsertPerson()
        {
            var manager = dataContext.Posts.First(p => p.PostName == "Manager");
            var assistant = dataContext.Posts.First(p => p.PostName == "Assistant");

            List<Person> people = new List<Person>();
            people.Add(new Person { PersonName = "Jan", Gender = "male", PostId = manager.Id });
            people.Add(new Person { PersonName = "Anna", Gender = "female", PostId = manager.Id });
            people.Add(new Person { PersonName = "John", Gender = "trans-gender", PostId = assistant.Id });
            people.Add(new Person { PersonName = "Mon", Gender = "female", PostId = assistant.Id });

            dataContext.Persons.InsertAllOnSubmit(people);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Persons.Select(x => new Entity.Person
            {
                Id = x.Id,
                Gender = x.Gender,
                PostId = x.PostId,
                Name = x.PersonName
            }).ToList();
        }

        public void InsertBenefit()
        {
            dataContext.BenefitLists.InsertOnSubmit(new BenefitList { BenefitName = "Sportcard" });
            dataContext.BenefitLists.InsertOnSubmit(new BenefitList { BenefitName = "Dentist" });

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.BenefitLists;
        }

        public void InsertPersonBenefitAssociations()
        {
            Person Jan = dataContext.Persons.First(p => p.PersonName == "Jan");
            Person Anna = dataContext.Persons.First(p => p.PersonName == "Anna");
            Person John = dataContext.Persons.First(p => p.PersonName == "John");
            Person Mon = dataContext.Persons.First(p => p.PersonName == "Mon");

            BenefitList Sportcard = dataContext.BenefitLists.First(b => b.BenefitName == "Sportcard");
            BenefitList Dentist = dataContext.BenefitLists.First(b => b.BenefitName == "Dentist");

            dataContext.PersonBenefits.InsertOnSubmit(new PersonBenefit { Person = Jan, BenefitList = Sportcard });
            dataContext.PersonBenefits.InsertOnSubmit(new PersonBenefit { Person = Anna, BenefitList = Sportcard });
            dataContext.PersonBenefits.InsertOnSubmit(new PersonBenefit { Person = Anna, BenefitList = Dentist });
            dataContext.PersonBenefits.InsertOnSubmit(new PersonBenefit { Person = John, BenefitList = Dentist });
            dataContext.PersonBenefits.InsertOnSubmit(new PersonBenefit { Person = Mon, BenefitList = Sportcard });

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.PersonBenefits;
        }
                
        private void BtnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            var addPersonDialog = new AddPersonDialog();
            addPersonDialog.ShowDialog();
        }

        private void ListPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var person = MainDataGrid.SelectedItem as Person;

            if (person == null)
            {
                return;
            }

            if (person != null)
            {
                TxtName.Text = person.PersonName;
                TxtGender.Text = person.Gender;
                TxtPost.Text = person.Post.PostName;
            }
        }

        private void BtnUpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            var person = MainDataGrid.SelectedItem as Person;

            if (person == null)
            {
                MessageBox.Show("Person must be selected before update!");
                return;
            }

            if (person != null)
            {
                person.PersonName = TxtName.Text;
                person.Gender = TxtGender.Text;

                var post = dataContext.Posts.FirstOrDefault();

                person.Post = post;

                dataContext.SubmitChanges();
            }

            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = dataContext.Persons.ToList();
            MainDataGrid.SelectedItem = person;
        }

        private void BtnDeletePerson_Click(object sender, RoutedEventArgs e)
        {
            var person = MainDataGrid.SelectedItem as Person;

            if (person == null)
            {
                MessageBox.Show("Person must be selected before delete!");
                return;
            }

            if (person != null)
            {
                dataContext.Persons.DeleteOnSubmit(person);
                dataContext.SubmitChanges();
                MainDataGrid.ItemsSource = null;
                MainDataGrid.ItemsSource = dataContext.Persons.ToList();

                TxtName.Text = string.Empty;
                TxtGender.Text = string.Empty;
                TxtPost.Text = string.Empty;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainDataGrid.ItemsSource = dataContext.Persons;
        }
    }
}
