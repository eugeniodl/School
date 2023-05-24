using Newtonsoft.Json;
using School.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School
{
    public partial class frmStudents : Form
    {
        public frmStudents()
        {
            InitializeComponent();
        }

        private void frmStudents_Load(object sender, EventArgs e)
        {
            GetAllStudents();
        }

        private async void GetAllStudents()
        {
            using(var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7222/api/Students");
                if(response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<StudentDto>>(students);
                    dgvStudents.DataSource = result.ToList();
                }
                else
                {
                    MessageBox.Show($"No se puede obtener la lista de estudiantes {response.StatusCode}");
                }
            }
        }
    }
}
