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
using ColorsManager.Controls;

namespace ColorsManager {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		
		private string _source = "data.txt";
		
		public MainWindow() {
			InitializeComponent();

			ButtonAdd.Click += ButtonAdd_Click;

			SliderBlue.ValueChanged += Slider_ValueChanged;
			SliderGreen.ValueChanged += Slider_ValueChanged;
			SliderRed.ValueChanged += Slider_ValueChanged;

			ButtonRandom.Click += ButtonRandom_Click;
			ButtonClear.Click += ButtonClear_Click;

			SliderGreen.IsEnabled = false;
			SliderRed.IsEnabled = false;
			SliderBlue.IsEnabled = false;

			Read();

			this.Closing += MainWindow_Closing;
		}

		private void ButtonClear_Click(Object sender, RoutedEventArgs e) {
			StackPanelColorControls.Children.Clear();
		}

		private void MainWindow_Closing(Object sender, System.ComponentModel.CancelEventArgs e) {
			Write();
		}

		private void Read() {
			try {
				StreamReader reader = new StreamReader(_source);

				string data = reader.ReadToEnd();

				foreach (var str in data.Split('\n')) {
					try {
						string rgb = str.Split('|')[0];
						string hex = str.Split('|')[1];
						string text = str.Split('|')[2];

						ControlColorCell newObj = new ControlColorCell();

						newObj.SelectControl += NewObj_SelectControl;

						newObj.RectangleColorBack.Fill = new SolidColorBrush(Color.FromRgb(byte.Parse(rgb.Split(',')[0]),
							byte.Parse(rgb.Split(',')[1]), byte.Parse(rgb.Split(',')[2])));

						newObj.FillChanged();

						newObj.Height = 50;

						newObj.DeleteControl += NewObj_DeleteControl;

						newObj.TextBlockHex.Text = hex;
						newObj.TextBoxText.Text = text;

						StackPanelColorControls.Children.Add(newObj);

					}
					catch{ }
				}

				reader.Close();
			}
			catch{ }
		}

		private void Write() {
			StreamWriter writer = new StreamWriter(_source,false);

			foreach (ControlColorCell obj in StackPanelColorControls.Children) {
				writer.WriteLine($"{obj.TextBlockRGBA.Text}|{obj.TextBlockHex.Text}|{obj.TextBoxText.Text}");
			}

			writer.Close();
		}

		private void ButtonRandom_Click(Object sender, RoutedEventArgs e) {
			ControlColorCell newObj = new ControlColorCell();

			newObj.SelectControl += NewObj_SelectControl;

			Random random = new Random(DateTime.Now.Millisecond);

			newObj.RectangleColorBack.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256)));

			newObj.FillChanged();

			newObj.Height = 50;

			newObj.DeleteControl += NewObj_DeleteControl;

			StackPanelColorControls.Children.Add(newObj);
		}

		private void Slider_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Double> e) {
			if (_selected == null) {
				return;
			}

			Color tempColor = ((System.Windows.Media.SolidColorBrush) (_selected.RectangleColorBack.Fill)).Color;

			tempColor.B = (byte)SliderBlue.Value;
			tempColor.G = (byte)SliderGreen.Value;
			tempColor.R = (byte)SliderRed.Value;

			TextBlockBlueValue.Text = tempColor.B.ToString();
			TextBlockGreenValue.Text = tempColor.G.ToString();
			TextBlockRedValue.Text = tempColor.R.ToString();

			_selected.RectangleColorBack.Fill = new SolidColorBrush(tempColor);

			_selected.FillChanged();
		}

		private ControlColorCell _selected = null;

		private void ButtonAdd_Click(Object sender, RoutedEventArgs e) {
			ControlColorCell newObj = new ControlColorCell();

			newObj.SelectControl += NewObj_SelectControl;

			newObj.FillChanged();

			newObj.Height = 50;

			newObj.DeleteControl += NewObj_DeleteControl;

			StackPanelColorControls.Children.Add(newObj);
		}

		private void NewObj_DeleteControl(ControlColorCell selectedColorCell) {
			StackPanelColorControls.Children.Remove(selectedColorCell);
		}

		private void NewObj_SelectControl(ControlColorCell selectedColorCell) {
			_selected = selectedColorCell;

			Color tempColor = ((System.Windows.Media.SolidColorBrush)(_selected.RectangleColorBack.Fill)).Color;

			SliderBlue.Value = tempColor.B;
			SliderGreen.Value = tempColor.G;
			SliderRed.Value = tempColor.R;
				
			SliderGreen.IsEnabled = true;
			SliderRed.IsEnabled = true;
			SliderBlue.IsEnabled = true;

			_selected.FillChanged();
		}
	}
}