using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kmz5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        byte [] K;
        int[] S;
        private void Button4_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            RandomKey();
        }

        void Sblock()
        {
            S = new int[256];
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
            }
            int t = 0;
            for (int i = 0; i < S.Length; i++)
            {
                int x = S[i];
                t = (t + S[i] + K[i%K.Length])%256;
                S[i] = S[t];
                S[t] = x;
            }
        }
        byte slovoK (int a)
        {
            int x = 0;
            int y = 0;
            byte Slovo = 0;
            for (int i = 0; i < a; i++)
            {
                x = (x + 1) % 256;
                y = (y + S[x]) % 256;
                int z = S[x];
                S[x] = S[y];
                S[y] = z;
                int t = (S[x] + S[y]) % 256;
                Slovo = Convert.ToByte(S[t]);
            }
            return (Slovo);
        }

        string Crypt()
        {
            string text = textBox1.Text;
            byte[] b = Encoding.GetEncoding(1251).GetBytes(text);
            for (int i = 0; i < text.Length; i++)
            {
                b[i] = XOR(Convert.ToByte(b[i]), slovoK(b.Length));
            }
            return (Encoding.GetEncoding(1251).GetString(b));
        }
        byte XOR(byte b1, byte b2)
        {
            string res = "";
            string s1 = Convert.ToString(Convert.ToInt32(b1), 2);
            string s2 = Convert.ToString(Convert.ToInt32(b2), 2);
            while(s1.Length != 8 || s2.Length != 8)
            {
                if (s1.Length != 8)
                    s1 = "0" + s1;
                if (s2.Length != 8)
                    s2 = "0" + s2;
            }
            for (int i = 0; i < 8; i++)
            {
                res += ((Convert.ToInt32(s1[i].ToString()) + Convert.ToInt32(s2[i].ToString())) % 2).ToString();
            }
            return (Convert.ToByte(res,2));
        }
        void RandomKey()
        {
            string key = "";
            Random rnd = new Random();
            int x = rnd.Next(20, 1025);
            while (x % 8 != 0)
                x++;
            x *= 2;
            for (int i = 0; i < x; i++)
            {
                key += Convert.ToString(rnd.Next(0, 2));
            }
            K = new byte[x/8];
            for (int i = 0; i < x/8; i++)
            {
                string tmp = "";
                for (int j = 0; j < 8; j++)
                {
                    tmp += key[i * 8 + j];
                }
                K[i] = Convert.ToByte(Convert.ToInt32(tmp,2));
            }
            for (int i = 0; i < K.Length; i++)
            {
                textBox3.Text += Convert.ToChar(K[i]);
                string tmp = K[i].ToString("x");
                if (tmp.Length < 2)
                    tmp = "0" + tmp;
                textBox4.Text += tmp;
            }
            label3.Text = "Размер ключа: " + x + " бит";
        }
        void enterKey()
        {
            string Hex = textBox4.Text;
            K = new byte[Hex.Length / 2];
            string[] tmp = new string[Hex.Length / 2];
            for (int i = 0; i < Hex.Length / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    tmp[i] += Hex[i * 2 + j];
                }
                K[i] = Convert.ToByte(Convert.ToInt32(tmp[i], 16));
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            enterKey();
            Sblock();
            textBox2.Text = Crypt();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
            textBox2.Clear();
        }

        private void TextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            textBox4.Clear();
            byte[] tmp = Encoding.GetEncoding(1251).GetBytes(textBox3.Text);
            for (int i = 0; i < tmp.Length; i++)
            {
                textBox4.Text += Convert.ToString(tmp[i], 16);
            }
            label3.Text = "Размер ключа: " + tmp.Length * 8 + " бит";
        }
    }
}
