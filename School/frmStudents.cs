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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private async void AddStudent()
        {
            StudentCreateDto studentDto = new StudentCreateDto();
            studentDto.StudentName = txtStudentName.Text;

            using(var client = new HttpClient())
            {
                var student = JsonConvert.SerializeObject(studentDto);
                var content = new StringContent(student, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:7222/api/Students", content);
                if (result.IsSuccessStatusCode)
                    MessageBox.Show("Estudiante agregado satifastoriamente");
                else
                    MessageBox.Show($"Error al guardar el estudiante: {result.Content.ReadAsStringAsync().Result}");
            }
        }
    }
}
