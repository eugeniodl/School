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
    public partial class frmStudent : Form
    {
        private static int id = 0;

        public frmStudent()
        {
            InitializeComponent();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            GetAllStudents();
        }

        private async void GetAllStudents()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:7184/api/Students"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var students = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<StudentDto>>(students);
                        dgvStudents.DataSource = result.ToList();
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
            StudentCreateDto studentCreateDto = new StudentCreateDto();
            studentCreateDto.StudentName = txtStudentName.Text;
            using (var client = new HttpClient())
            {
                var serializedStudent = JsonConvert.SerializeObject(studentCreateDto);
                var content = new StringContent(serializedStudent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7184/api/Students", content);
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Estudiante agregado");
                else
                    MessageBox.Show($"Error al guardar el estudiante {response.Content.ReadAsStringAsync().Result}");
            }
            Clear();
            GetAllStudents();
        }

        private void Clear()
        {
            txtStudentId.Text = string.Empty;
            txtStudentName.Text = string.Empty;
            id = 0;
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Index == e.RowIndex)
                {
                    id = int.Parse(row.Cells[0].Value.ToString());
                    GetStudentById(id);
                }
            }
        }

        private async void GetStudentById(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(String.Format("{0}/{1}",
                    "https://localhost:7184/api/Students", id));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    StudentDto studentDto = JsonConvert.DeserializeObject<StudentDto>(data);
                    txtStudentId.Text = studentDto.StudentId.ToString();
                    txtStudentName.Text = studentDto.StudentName;
                }
                else
                {
                    MessageBox.Show($"No se puede obtener el estudiante: {response.StatusCode}");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (id != 0)
                UpdateStudent();
        }

        private async void UpdateStudent()
        {
            StudentUpdateDto studentDto = new StudentUpdateDto();
            studentDto.StudentId = id;
            studentDto.StudentName = txtStudentName.Text;

            using (var client = new HttpClient())
            {
                var student = JsonConvert.SerializeObject(studentDto);
                var content = new StringContent(student, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(String.Format("{0}/{1}",
                    "https://localhost:7184/api/Students", id), content);
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Estudiante actualizado");
                else
                    MessageBox.Show($"Error al actualizar el estudiante: {response.StatusCode}");
            }
            Clear();
            GetAllStudents();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (id != 0)
                DeleteStudent();
        }

        private async void DeleteStudent()
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7184/api/Students");
                var response = await client.DeleteAsync(String.Format("{0}/{1}",
                    "https://localhost:7184/api/Students", id));
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Estudiante eliminado con éxito");
                else
                    MessageBox.Show($"No se pudo eliminar el estudiante: {response.StatusCode}");
            }
            Clear();
            GetAllStudents();
        }
    }
}