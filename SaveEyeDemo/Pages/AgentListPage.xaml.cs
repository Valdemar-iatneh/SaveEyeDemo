using SaveEyeDemo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace SaveEyeDemo.Pages
{
    /// <summary>
    /// Interaction logic for AgentListPage.xaml
    /// </summary>
    public partial class AgentListPage : Page
    {
        private List<Agent> agents = new List<Agent>();
        private List<AgentType> agentTypes = new List<AgentType>();
        public AgentListPage()
        {
            InitializeComponent();
            agents = DBConnection.connection.Agent.ToList();
            agentTypes = DBConnection.connection.AgentType.ToList();
            agentTypes.Insert(0, new AgentType { ID = -1, Title = "Все типы" });
            typeCB.ItemsSource = agentTypes;    
            typeCB.SelectedIndex = 0;
            sortingCB.SelectedIndex = 0;

            foreach(Agent agent in agents)
            {
                if (agent.Logo!= null)
                {
                    agent.LogoImage = File.ReadAllBytes(@"C:\Users\Waldemar\source\repos\SaveEyeDemo\SaveEyeDemo\Resources\agents\" + agent.Logo);
                    //save
                }
            }
            agentLV.ItemsSource = agents;
        }

        private void ToFilter()
        {
            var filteredAgents = (IEnumerable<Agent>)DBConnection.connection.Agent.ToList();
            if (!string.IsNullOrWhiteSpace(searchTB.Text)) 
            {
                filteredAgents = filteredAgents.Where
                (
                    x => x.Title.StartsWith(searchTB.Text) || 
                    x.Email.StartsWith(searchTB.Text) ||
                    x.Phone.StartsWith(searchTB.Text)
                );
            }

            if (typeCB.SelectedIndex > 0)
            {
                filteredAgents = filteredAgents.Where(x => x.AgentTypeID == (typeCB.SelectedItem as AgentType).ID);
            }

            switch (sortingCB.SelectedIndex)
            {
                case 0:
                    filteredAgents = filteredAgents.OrderBy(x => x.ID);
                    break;
                case 1:
                    filteredAgents = filteredAgents.OrderBy(x => x.Title);
                    break;
                case 2:
                    filteredAgents = filteredAgents.OrderByDescending(x => x.Title);    
                    break;
                case 3:
                    filteredAgents = filteredAgents.OrderBy(x => x.Discount);
                    break;
                case 4:
                    filteredAgents = filteredAgents.OrderByDescending(x => x.Discount);
                    break;
                case 5:
                    filteredAgents = filteredAgents.OrderBy(x => x.Priority);
                    break;
                case 6:
                    filteredAgents = filteredAgents.OrderByDescending(x => x.Priority);
                    break;
            }
            agentLV.ItemsSource = filteredAgents;
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToFilter();
        }

        private void sortingCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToFilter();
        }

        private void typeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToFilter();
        }
    }
}
