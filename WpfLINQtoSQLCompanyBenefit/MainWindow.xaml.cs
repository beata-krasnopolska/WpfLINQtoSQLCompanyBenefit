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

            InsertPost();
            //InsertPerson();
            //InsertBenefit();
            //InsertPersonBenefitAssociations();
            //GetPostOfAnna();
            //GetAnnaBenefit();
            //GetAllManagers();
            //GetAllPostsWithTransgenders();
            //GetAssistantsBenefits();
            //UpdateMon();
            //DeleteJohn();
        }

        public void InsertPost()
        {
            dataContext.ExecuteCommand("delete from Post");

            Post Manager = new Post();
            Manager.PostName = "Manager";
            dataContext.Posts.InsertOnSubmit(Manager);

            Post Assistant = new Post();
            Assistant.PostName = "Assistant";
            dataContext.Posts.InsertOnSubmit(Assistant);

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Posts;
        }

        public void InsertPerson()
        {
            Post Manager = dataContext.Posts.First(p => p.PostName == "Manager");
            Post Assistant = dataContext.Posts.First(p => p.PostName == "Assistant");

            List<Person> people = new List<Person>();
            people.Add(new Person { PersonName = "Jan", Gender = "male", Post = Manager });
            people.Add(new Person { PersonName = "Anna", Gender = "female", Post = Manager });
            people.Add(new Person { PersonName = "John", Gender = "trans-gender", Post = Assistant });
            people.Add(new Person { PersonName = "Mon", Gender = "female", Post = Assistant });

            dataContext.Persons.InsertAllOnSubmit(people);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Persons;
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

        public void GetPostOfAnna()
        {
            Person Anna = dataContext.Persons.First(p => p.PersonName == "Anna");

            Post post = Anna.Post;
            List<Post> annaPost = new List<Post>();
            annaPost.Add(post);
            MainDataGrid.ItemsSource = annaPost;
        }

        public void GetAnnaBenefit()
        {
            var annasBenefit = from pb in dataContext.PersonBenefits
                               where pb.Person.PersonName == "Anna"
                               select pb.BenefitList;

            MainDataGrid.ItemsSource = annasBenefit;
        }

        public void GetAllManagers()
        {
            var managerPeople = from person in dataContext.Persons
                                where person.Post.PostName == "Manager"
                                select person;

            MainDataGrid.ItemsSource = managerPeople;
        }

        public void GetAllPostsWithTransgenders()
        {
            var transgenders = from person in dataContext.Persons
                               join post in dataContext.Posts
                               on person.Post equals post
                               where person.Gender == "trans-gender"
                               select post;

            MainDataGrid.ItemsSource = transgenders;
        }

        public void GetAssistantsBenefits()
        {
            var assistantsBenefits = from personBenefit in dataContext.PersonBenefits
                                     join person in dataContext.Persons
                                     on personBenefit.PersonId equals person.Id
                                     where person.Post.PostName == "Assistant"
                                     select personBenefit.BenefitList;

            MainDataGrid.ItemsSource = assistantsBenefits;
        }

        public void UpdateMon()
        {
            Person Mon = dataContext.Persons.FirstOrDefault(p => p.PersonName == "Mon");

            Mon.PersonName = "Monica";
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Persons;
        }

        public void DeleteJohn()
        {
            Person John = dataContext.Persons.FirstOrDefault(p => p.PersonName == "John");
            dataContext.Persons.DeleteOnSubmit(John);

            dataContext.SubmitChanges();

            string connectionString = ConfigurationManager.ConnectionStrings["WpfLINQtoSQLCompanyBenefit.Properties.Settings.CompanyBenefitDBConnectionString"].ConnectionString;
            dataContext = new LINQtoSQLdataClassesDataContext(connectionString);

            MainDataGrid.ItemsSource = dataContext.Persons;
        }
    }
}
