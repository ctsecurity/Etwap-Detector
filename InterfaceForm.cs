﻿using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Etwap_Detector
{
    public partial class InterfaceForm : Form
    {
        private static Wifi wifi;

        public InterfaceForm()
        {
            InitializeComponent();
        }

        private void InterfaceForm_Load(object sender, EventArgs e)
        {
            wifi = new Wifi();

            List<AccessPoint> aps = wifi.GetAccessPoints();

            foreach (AccessPoint ap in aps)
            {
                ListViewItem listView = new ListViewItem(ap.Name);
                listView.SubItems.Add(ap.SignalStrength + "%");
                listView.SubItems.Add(ap.IsSecure.ToString());

                listView.Tag = ap;

                listView_AP.Items.Add(listView);

                if (wifi.ConnectionStatus == WifiStatus.Connected)
                {
                    lbl_Status.Text = ap.Name;
                    btn_Connect.Enabled = false;
                    txBox_Password.Enabled = false;
                    btn_Disconnect.Enabled = true;
                }
                else
                {
                    lbl_Status.Text = "Not connected.";
                    btn_Connect.Enabled = true;
                    txBox_Password.Enabled = true;
                    btn_Disconnect.Enabled = false;
                }
            }
        }

        private void Btn_Connect_Click(object sender, EventArgs e)
        {
            if (listView_AP.SelectedItems.Count > 0 && txBox_Password.Text.Length > 0)
            {
                ListViewItem selectedItem = listView_AP.SelectedItems[0];

                AccessPoint ap = (AccessPoint)selectedItem.Tag;

                if (Connecter(ap, txBox_Password.Text))
                {
                    lbl_Status.Text = ap.Name;
                    btn_Connect.Enabled = false;
                    txBox_Password.Enabled = false;
                    btn_Disconnect.Enabled = true;
                }

                lbl_Status.Text = "Not connected.";
                btn_Connect.Enabled = true;
                txBox_Password.Enabled = true;
                btn_Disconnect.Enabled = false;
            }
        }

        private bool Connecter(AccessPoint ap, string password)
        {
            AuthRequest authRequest = new AuthRequest(ap)
            {
                Password = password
            };

            return ap.Connect(authRequest);
        }

        private void Btn_Disconnect_Click(object sender, EventArgs e)
        {
            if (wifi.ConnectionStatus == WifiStatus.Connected)
                wifi.Disconnect();
        }
    }
}