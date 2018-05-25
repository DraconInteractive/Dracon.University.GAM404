using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GAM404Exercise1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string username, description;
        public int level;
        public float experiencePercent;
        public string testString = "red";
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            username = USERNAME.Text;
            description = DESCRIPTION.Text;
            level = int.Parse(LEVEL.Text);
            experiencePercent = float.Parse(EXPERIENCE.Text);

            Character createdCharacter = new Character(username, description, level, experiencePercent);
            CharacterSave.savedCharacter = createdCharacter;

            Window1 win1 = new Window1();
            win1.Show();
            this.Close();
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(username)));
        }
    }

    public class Character
    {
        public string username, description;
        public int level;
        public float experiencePercent;

        public Character (string u, string d, int l, float e)
        {
            username = u;
            description = d;
            level = l;
            experiencePercent = e;
        }
    }

    public static class CharacterSave
    {
        public static Character savedCharacter;
    }
}
