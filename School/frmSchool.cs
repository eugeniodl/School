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
    public partial class frmSchool : Form
    {
        public frmSchool()
        {
            InitializeComponent();
        }

        private void frmSchool_Load(object sender, EventArgs e)
        {
            GetAllStudents();
        }

        private async void GetAllStudents()
        {
            using(var client = new HttpClient())
            {
                using(var response = await client.GetAsync("https://localhost:7286/api/Student"))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        var students = await response.Content.ReadAsStringAsync();
                        var displaydata = JsonConvert.DeserializeObject<List<StudentDto>>(students);
                        dgvStudents.DataSource = displaydata.ToList();
                    }
                    else
                    {
                        MessageBox.Show($"No se puede obtener la lista de estudiantes: {response.StatusCode}");
                    }
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private async void AddStudent()
        {
            StudentCreateDto oStudent = new StudentCreateDto();
            oStudent.StudentName = txtStudentName.Text;
            using(var client = new HttpClient())
            {
                var serializedStudent = JsonConvert.SerializeObject(oStudent);
                var content = new StringContent(serializedStudent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7286/api/Student", content);
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Estudiante agregado");
                else
                    MessageBox.Show($"Error al guardar el estudiante: {response.Content.ReadAsStringAsync().Result}");
            }
            Clear();
            GetAllStudents();
        }

        private void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
