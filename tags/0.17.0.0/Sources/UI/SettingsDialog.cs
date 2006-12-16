// Copyright (c) 2006, Eugene Rymski
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted 
//  provided that the following conditions are met:
// * Redistributions of source code must retain the above copyright notice, this list of conditions 
//   and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
//   and the following disclaimer in the documentation and/or other materials provided with the distribution.
// * Neither the name of the “Varozhka” nor the names of its contributors may be used to endorse or 
//   promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY 
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Windows.Forms;
using Varozhka.UI.Properties;

namespace Varozhka.UI
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void _btnChangeNetflixDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(_boxNexflixDir, "Select directory with Netflix dataset");
        }

        private void SelectDirectory(TextBox field, string description)
        {
            _dlgFolderBrowser.Description = description;
            DialogResult result = _dlgFolderBrowser.ShowDialog();
            if (DialogResult.OK == result)
            {
                field.Text = _dlgFolderBrowser.SelectedPath;
            }
        }

        private void _btnChangeOutputDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(_boxOutputDir, "Select output directory");
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }

        private void _btnAssembly_Click(object sender, EventArgs e)
        {
            DialogResult result = _dlgOpenFile.ShowDialog();
            if (DialogResult.OK == result)
            {
                _boxAssembly.Text = _dlgOpenFile.FileName;
            }
        }
    }
}