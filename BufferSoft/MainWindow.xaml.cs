using System;
using System.Collections.Generic;
using System.Windows.Interop;
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
using System.Runtime.InteropServices;
using InputSimulatorStandard;

namespace BufferSoft
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWn,int id);

        //
        private WindowInteropHelper _WIH;
        private HwndSource _HWNDsource;
        private HwndSourceHook _Hook;


        //необходимые константы
        public const int MOD_ALT = 0x1;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public const int WM_HOTKEY = 0x312;

        //несколько примеров виртуальных кодов
        public const uint VK_V = 0x56;

        private bool _SimpleWork = false;
        private string _Simpletext;
        private int _Simplenumber;
        private int _Simpleincome;
        //hardcopytext
        private bool _HardWork = false;
        private string _Hardtextstart;
        private string _Hardtextend;
        private int _Hardnumber;
        private int _Hardincome;

        private string BufferString
        {
            get
            {
                return BufferString;
            }
            set
            {
                BufferString = value;
                Clipboard.SetText(value);
            }
        }

        InputSimulator _input = new InputSimulator();
        //обработчик сообщений для окна
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            if (msg == WM_HOTKEY)
            {
                if (_SimpleWork)
                {
                    string str = string.Empty;

                    str += _Simpletext;
                    str += _Simplenumber + _Simpleincome;
                    _Simplenumber = _Simplenumber + _Simpleincome;


                    _input.Keyboard.TextEntry(str);
                    Clipboard.SetText(str);
                }
                else if (_HardWork)
                {
                    string str = string.Empty;

                    str += _Hardtextstart;
                    str += _Hardnumber + _Hardincome;
                    _Hardnumber = _Hardnumber + _Hardincome;
                    str += _Hardtextend;

                    _input.Keyboard.TextEntry(str);
                    Clipboard.SetText(str);
                }

            }

            return IntPtr.Zero;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            _WIH = new WindowInteropHelper(this);
            _HWNDsource = HwndSource.FromHwnd(_WIH.Handle);
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Window.Topmost = CheckBox1.IsChecked.Value;
        }
        private void SimpleButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (SimpleButtonStart.Content.ToString() == "Старт")
            {
                SimpleButtonStart.Content = "Стоп";

                _SimpleWork = true;
                _Simpletext = srctext.Text;
                _Simplenumber = Convert.ToInt32(srcnum.Text);
                _Simpleincome = Convert.ToInt32(income.Text);

                _Hook = new HwndSourceHook(WndProc);
                _HWNDsource.AddHook(_Hook);//регистрируем обработчик сообщений
                bool res = RegisterHotKey(_WIH.Handle, 1, MOD_CONTROL, VK_V);//регистрируем горячую клавишу
                if (res == false) MessageBox.Show("RegisterHotKey failed");
            }
            else if (SimpleButtonStart.Content.ToString() == "Стоп")
            {
                SimpleButtonStart.Content = "Старт";
                _HWNDsource.RemoveHook(_Hook);
                _SimpleWork = false;

                bool res = UnregisterHotKey(_WIH.Handle, 1);//регистрируем горячую клавишу
                if (res == false) MessageBox.Show("RegisterHotKey failed");
            }
        }

        private void HardButtonStart_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (HardButtonStart_Copy.Content.ToString() == "Старт")
            {
                HardButtonStart_Copy.Content = "Стоп";

                _HardWork = true;
                _Hardtextstart = HardTextStart.Text;
                _Hardtextend = HardTextEnd.Text;
                _Hardnumber = Convert.ToInt32(HardNumber.Text);
                _Hardincome = Convert.ToInt32(HardIncome.Text);

                _Hook = new HwndSourceHook(WndProc);
                _HWNDsource.AddHook(_Hook);//регистрируем обработчик сообщений
                bool res = RegisterHotKey(_WIH.Handle, 1, MOD_CONTROL, VK_V);//регистрируем горячую клавишу
                if (res == false) MessageBox.Show("RegisterHotKey failed");
            }
            else if (HardButtonStart_Copy.Content.ToString() == "Стоп")
            {
                HardButtonStart_Copy.Content = "Старт";
                _HWNDsource.RemoveHook(_Hook);
                _HardWork = false;

                bool res = UnregisterHotKey(_WIH.Handle, 1);//регистрируем горячую клавишу
                if (res == false) MessageBox.Show("RegisterHotKey failed");
            }
        }
    }
}