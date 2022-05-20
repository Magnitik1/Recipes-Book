using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

namespace Recipes_book;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        update();
    }
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    public static List<string> files = new List<string>();
    public static string min_t;
    public static string max_t;
    public static string main_ingr;
    public static string type;
    public void update() {
        var t = Directory.GetFiles("ingr/");
        q.Items.Clear();
        files.Clear();
        foreach (var f in t) {
            StringBuilder sb = new StringBuilder();
            for (int i = 5; i < f.Length - 4; i++) { sb.Append(f[i]); }
            files.Add(sb.ToString());
            q.Items.Add(sb.ToString());
        }
        q.FontSize = 25;
        ListBoxItem item = new ListBoxItem() {
            Content = " +  Add New Recipe",
            Foreground = new SolidColorBrush(Colors.LimeGreen),
            Height = 60,
            FontWeight = FontWeights.Bold
        };
        q.Items.Add(item);
    }
    public static string way;
    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (q.Items.Count <= 0) return;
        string s = (sender as ListBox).SelectedValue.ToString();
        if (s == "System.Windows.Controls.ListBoxItem:  +  Add New Recipe") {
            Window1 win = new Window1();
            win.but.Visibility = Visibility.Hidden;
            win.cancel.Visibility = Visibility.Visible;
            win.save.Visibility = Visibility.Visible;
            win.name.IsReadOnly = false;
            win.ingridients.IsReadOnly = false;
            win.recipe.IsReadOnly = false;
            win.time.IsReadOnly = false;
            win.type.IsEnabled = true;
            Window1.AddNew = true;
            Close();
            win.Show();
        }
        else {
            way = s;
            StreamReader sr = new StreamReader("ingr/" + way + ".txt");
            string all = sr.ReadToEnd();
            sr.Close();
            string name = GetName(all);
            string ingridients = GetIngridients(all);
            string recipe = GetRecipe(all);
            string type3 = GetType(all);
            string time = GetTime(all);
            min_t = mint.Text;
            max_t = maxt.Text;
            type = type2.Text;
            main_ingr = mainprod.Text.Trim();
            Window1 win = new Window1();
            win.name.Text = name;
            win.recipe.Text = recipe;
            win.ingridients.Text = ingridients;
            win.time.Text = time;
            win.type.Text = type3;
            win.Show();
            Close();
        }
    }

    private string GetTime(string all) {
        var temp = new StringBuilder();
        int i = all.IndexOf(":&$&(") + 6;
        int end = all.IndexOf("%^*$^&") - 1;
        for (int y = i; i < end; i++) {
            temp.Append(all[i]);
        }
        return new string(temp.ToString());
    }

    private string GetType(string all) {
        var temp = new StringBuilder();
        int i = all.IndexOf(":#$%^") + 6;
        int end = all.IndexOf("(&$^&") - 1;
        for (int y = i; i < end; i++) {
            temp.Append(all[i]);
        }
        return new string(temp.ToString());
    }

    private string GetRecipe(string all) {
        var temp = new StringBuilder();
        int i = all.Trim('\r').IndexOf("*&*)(*&^") + 10;
        int end = all.Length - 1;
        for (int y = i; i < end; i++) {
            temp.Append(all[i]);
        }
        return new string(temp.ToString());
    }

    private string GetIngridients(string all) {
        var temp = new StringBuilder();
        int i = all.Trim('\r').IndexOf("*#$%@") + 7;
        int end = all.IndexOf("*&&*$%");
        for (int y = i; i < end; i++) {
            temp.Append(all[i]);
        }
        return new string(temp.ToString());
    }

    private string GetName(string all) {
        var temp = new StringBuilder();
        for (int i = 6; i < all.Length; i++) {
            if (all[i] == '\r') break;
            temp.Append(all[i]);
        }
        return new string(temp.ToString());
    }
    private void q_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
        ListBox_SelectionChanged(sender, null);
    }

    private void CleanF(object sender, RoutedEventArgs e) {
        mint.Text = "";
        maxt.Text = "";
        mainprod.Text = "";
        type2.SelectedIndex = 6;
        update();
    }

    public void Search(object sender, RoutedEventArgs e) {
        var t = Directory.GetFiles("ingr/");
        q.Items.Clear();
        foreach (var f in t) {
            StreamReader sr = new StreamReader(f);
            string s = sr.ReadToEnd();
            int ti = Convert.ToInt32(GetTime(s).Trim('х', 'в', '.', ' '));
            string ing = GetIngridients(s);
            string typ = GetType(s);
            bool no_min = false, no_max = false, no_ingr = false, no_type = false;
            int mi = 99999, ma = 0;
            if (mint.Text != null && mint.Text.Trim() != "") mi = Convert.ToInt32(mint.Text.Trim()); else no_min = true;
            if (maxt.Text != null && maxt.Text.Trim() != "") ma = Convert.ToInt32(maxt.Text.Trim()); else no_max = true;
            try {
                if (mainprod.Text != null && mainprod.Text.Trim() != "")
                    no_ingr = ing.ToLower().Contains(mainprod.Text.ToLower());
                else no_ingr = true;
            }
            catch { no_ingr = true; }
            if (type2.Text == typ || type2.Text == "Будь-які") { no_type = true; }
            if ((ti >= mi || no_min) && (ti <= ma || no_max) && (no_ingr) && (no_type)) {
                q.Items.Add(GetName(s));
            }
        }
        ListBoxItem item = new ListBoxItem() {
            Content = " +  Add New Recipe",
            Foreground = new SolidColorBrush(Colors.LimeGreen),
            Height = 60,
            FontWeight = FontWeights.Bold
        };
        q.Items.Add(item);

    }
    private void random(object sender, RoutedEventArgs e) {
        Random rnd = new Random();
        q.SelectedIndex=rnd.Next(0,q.Items.Count-1);
    }
}


