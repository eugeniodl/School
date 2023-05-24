﻿using Newtonsoft.Json;
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
        private static int id = 0;

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
                var response = await client.GetAsync("https://localhost:7262/api/Students");
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

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private async void AddStudent()
        {
            StudentCreateDto studentDto = new StudentCreateDto();
            studentDto.StudentName = txtStudentName.Text;

            using (var client = new HttpClient())
            {
                var serializedStudent = JsonConvert.SerializeObject(studentDto);
                var content = new StringContent(serializedStudent, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:7262/api/Students", content);
                if(result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Estudiante agregado correctamente");
                }
                else
                {
                    MessageBox.Show($"Error al guardar el estudiante: {result.Content.ReadAsStringAsync().Result}");
                }
                Clear();
                GetAllStudents();
            }
        }

        private void Clear()
        {
            txtStudentId.Text = string.Empty;
            txtStudentName.Text = string.Empty;
            id = 0;
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach(DataGridViewRow row in dgvStudents.Rows)
            {
                if(row.Index == e.RowIndex)
                {
                    id = int.Parse(row.Cells[0].Value.ToString());
                    GetStudentById(id);
                }
            }
        }

        private async void GetStudentById(int id)
        {
            using(var client = new HttpClient())
            {
                var response = await client.GetAsync(String.Format("{0}/{1}", "https://localhost:7262/api/Students",id));
                if(response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadAsStringAsync();
                    StudentDto studentDto = JsonConvert.DeserializeObject<StudentDto>(student);
                    txtStudentId.Text = studentDto.StudentId.ToString();
                    txtStudentName.Text = studentDto.StudentName;
                }
                else
                {
                    MessageBox.Show($"No se puede obtener el estudiante: {response.StatusCode}");
                }
            }
        }
    }
}
