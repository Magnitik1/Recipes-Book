using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Recipes_book;
/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class Window1 : Window {
    public Window1() {
        InitializeComponent();
    }
    public static string orig_name;
    public static string orig_ingridients;
    public static string orig_recipe;
    public static string orig_type;
    public static string orig_time;
    public static bool AddNew = false;
    private void Button_Click(object sender, RoutedEventArgs e) {
        but.Visibility = Visibility.Hidden;
        orig_name = name.Text;
        orig_ingridients = ingridients.Text;
        orig_recipe = recipe.Text;
        var sb = new StringBuilder();
        int u = time.Text.IndexOf(" хв.");
        for (int i = 0; i < u; i++) {
            sb.Append(time.Text[i]);
        }
        orig_time = time.Text;
        time.Text = sb.ToString();
        orig_type = type.Text;
        cancel.Visibility = Visibility.Visible;
        save.Visibility = Visibility.Visible;
        delete.Visibility = Visibility.Visible;
        recipe.IsReadOnly = false;
        ingridients.IsReadOnly = false;
        name.IsReadOnly = false;
        type.IsEnabled = true;
        time.IsReadOnly = false;
    }
    private void cancel_Click(object sender, RoutedEventArgs e) {
        name.Text = orig_name;
        if(name.Text == "") {
            Close();
            return;
        }
        but.Visibility = Visibility.Visible;
        name.IsReadOnly = true;
        recipe.Text = orig_recipe;
        recipe.IsReadOnly = true;
        ingridients.Text = orig_ingridients;
        ingridients.IsReadOnly = true;
        type.Text = orig_type;
        type.IsEnabled = false;
        time.Text = orig_time;
        time.IsReadOnly = true;
        cancel.Visibility = Visibility.Hidden;
        save.Visibility = Visibility.Hidden;
        delete.Visibility = Visibility.Hidden;
    }
    private void NameValidationTextBox(object sender, TextCompositionEventArgs e) {
        Regex regex = new Regex("[^a-z A-Z а-я А-Я 0-9 і ї І Ї]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    private void timeValidationTextBox(object sender, TextCompositionEventArgs e) {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    private void save_Click(object sender, RoutedEventArgs e) {
        if(time.Text == null||time.Text=="") return;
        if(ingridients.Text == null||ingridients.Text=="") return;
        if(recipe.Text == null|| recipe.Text=="") return;
        if(type.Text == null||type.Text=="") return;
        if (name.Text.Trim() == "" || name.Text == null || (MainWindow.files.Contains(name.Text) || name.Text==" +  Add New Recipe") && (orig_name != name.Text||AddNew)) { return; }
        AddNew = false;
        time.Text = time.Text.Trim();
        time.Text += " хв.";
        if (name.Text != orig_name&&orig_name!=null) {
            File.Delete("ingr/" + orig_name + ".txt");
        }
        StreamWriter sw = new StreamWriter("ingr/" + name.Text + ".txt");
        sw.WriteLine("Name: " + name.Text);
        sw.WriteLine("Ingridients: *#$%@\r " + ingridients.Text + "\r*&&*$%");
        sw.WriteLine("Type:#$%^ " + type.Text + " (&$^&");
        sw.WriteLine("Time:&$&( " + time.Text + " %^*$^&");
        sw.WriteLine("Recepie: *&*)(*&^\r " + recipe.Text);
        sw.Close();
        Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
        MainWindow mw = new MainWindow();
        if (MainWindow.min_t == null) mw.mint.Text = "";
        else if (MainWindow.min_t.ToString() != "0") mw.mint.Text = MainWindow.min_t; 
        if (MainWindow.max_t == null) mw.maxt.Text = "";
        else if (MainWindow.max_t.ToString() != "0") mw.maxt.Text = MainWindow.max_t;
        if (MainWindow.type == null) mw.type2.Text = "Будь-які";
        else mw.type2.Text=MainWindow.type;
        mw.mainprod.Text = MainWindow.main_ingr;
        mw.Show();
        mw.Search(null, null);
    }
    private void del(object sender, RoutedEventArgs e) {
        if(MessageBox.Show("Are you shure?", "", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes){
            File.Delete("ingr/" + orig_name + ".txt");
            Close();
        }
    }
}
