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

namespace ColorsManager.Controls {
	/// <summary>
	/// Логика взаимодействия для ControlColorCell.xaml
	/// </summary>
	public partial class ControlColorCell : UserControl {
		public delegate void BaseDelegate(ControlColorCell selectedColorCell);

		public event BaseDelegate SelectControl;
		public event BaseDelegate DeleteControl;

		public ControlColorCell() {
			InitializeComponent();

			ButtonSelect.Click += ButtonSelect_Click;
			MenuItemDelete.Click += MenuItemDelete_Click;

			TextBlockHex.MouseLeftButtonDown += TextBlockHex_MouseLeftButtonDown;
			TextBlockRGBA.MouseLeftButtonDown += TextBlockRGBA_MouseLeftButtonDown;
		}

		private void TextBlockRGBA_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e) {
			Clipboard.SetText(TextBlockRGBA.Text);
			PopupBuffer.IsOpen = true;
		}

		private void TextBlockHex_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e) {
			Clipboard.SetText(TextBlockHex.Text);
			PopupBuffer.IsOpen = true;
		}

		private void MenuItemDelete_Click(Object sender, RoutedEventArgs e) {
			DeleteControl?.Invoke(this);
		}

		public void FillChanged() {
			Color tempColor = ((System.Windows.Media.SolidColorBrush) (RectangleColorBack.Fill)).Color;

			TextBlockHex.Text = tempColor.ToString().Remove(1, 2);
			TextBlockRGBA.Text = $"{tempColor.R},{tempColor.G},{tempColor.B}";

			byte middle(byte value) {
				if (value > 108 && value < 148) {
					return 0;
				}
				else {
					return value;
				}
			}

			SolidColorBrush tempContrColor = new SolidColorBrush(Color.FromArgb(255, (byte) (~middle(tempColor.R)),
				(byte) (~middle(tempColor.G)), (byte) (~middle(tempColor.B))));

			TextBlockHex.Foreground = tempContrColor;
			TextBlockRGBA.Foreground = tempContrColor;
			TextBoxText.Foreground = tempContrColor;
		}

		private void ButtonSelect_Click(Object sender, RoutedEventArgs e) {
			SelectControl?.Invoke(this);
		}
	}
}