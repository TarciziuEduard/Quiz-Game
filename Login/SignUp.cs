﻿/*************************************************************************
* *
* File: SignUp.cs *
* Descriere: Functionalitatea formularului de login. Se introduce utilizatorul si parola, daca exista contul se trece pe alegerea domeniilor.*
* In caz ca nu exista contul se poate crea unul.*
* Autor: Cristi Sandu *
*************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Login
{
    public class SignUp
    {
        private string _email;
        private string _nume;
        private string _parola;
        private string _numeFisier = @"../../utilizatori.json";

        // Validare adresă de e-mail
        private string _emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Validare parolă cu cel puțin 8 caractere
        private string _parolaPattern = @".{8,}";

        /// <summary>
        /// Constructor pentru clasa SignUp.
        /// Inițializează emailul, numele și parola pentru instanța curentă și verifică validitatea acestora.
        /// </summary>
        /// <param name="email">Emailul utilizatorului.</param>
        /// <param name="nume">Numele utilizatorului.</param>
        /// <param name="parola">Parola utilizatorului.</param>
        public SignUp(string email, string nume, string parola)
        {
            _email = email;
            _parola = parola;
            _nume = nume;
            VerificareCampuri();
        }

        /// <summary>
        /// Verifică validitatea datelor introduse.
        /// Aruncă o excepție dacă datele nu sunt valide.
        /// </summary>
        private void VerificareCampuri()
        {
            if (string.IsNullOrWhiteSpace(_nume) || string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_parola))
            {
                throw new Exception("Vă rugăm să completați toate câmpurile.");
            }
            else if (!Regex.IsMatch(_email, _emailPattern))
            {
                throw new Exception("Adresa de e-mail introdusă nu este validă.");
            }
            else if (!Regex.IsMatch(_parola, _parolaPattern))
            {
                throw new Exception("Parola trebuie să aibă cel puțin 8 caractere.");
            }

        }

        /// <summary>
        /// Creează un cont nou de utilizator.
        /// Dacă fișierul nu există, acesta este creat.
        /// Dacă fișierul există deja, noul utilizator este adăugat la listă.
        /// </summary>
        public void CreateAccount()
        {
            // Verificăm dacă fișierul utilizatori.json există deja
            if (!File.Exists(_numeFisier))
            {
                // Dacă nu există, creăm un nou fișier JSON cu obiectul Utilizator serializat
                Utilizator utilizator = new Utilizator()
                {
                    Nume = _nume,
                    Email = _email,
                    Parola = _parola
                };
                string json = JsonConvert.SerializeObject(utilizator, Formatting.Indented);
                File.WriteAllText(_numeFisier, json);
            }
            else
            {
                // Dacă fișierul există deja, încărcăm datele vechi
                string jsonVechi = File.ReadAllText(_numeFisier);
                List<Utilizator> utilizatori = JsonConvert.DeserializeObject<List<Utilizator>>(jsonVechi);

                bool utilizatorExistent = utilizatori.Any(u => u.Email == _email);
                if (utilizatorExistent)
                {
                    throw new Exception("Exista deja un utilizator cu acest email.");
                }
                else
                {
                    // Adăugăm datele noi la lista de utilizatori
                    utilizatori.Add(new Utilizator
                    {
                        Nume = _nume,
                        Email = _email,
                        Parola = _parola
                    });

                    // Serializăm lista actualizată și rescriem fișierul JSON
                    string jsonNou = JsonConvert.SerializeObject(utilizatori, Formatting.Indented);
                    File.WriteAllText(_numeFisier, jsonNou);
                }
            }
        }
    }
}
