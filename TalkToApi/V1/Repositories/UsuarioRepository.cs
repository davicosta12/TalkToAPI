﻿using Microsoft.AspNetCore.Identity;
using TalkToApi.V1.Models;
using TalkToApi.V1.Repositores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasTarefasApi.Repositores
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser Obter(string email, string senha)
        {
            var usuario = _userManager.FindByEmailAsync(email).Result;
            if (_userManager.CheckPasswordAsync(usuario, senha).Result)
            {
                return usuario;
            }
            else
            {
                throw new Exception("Usuário não localizado");
            }
        }

        ApplicationUser IUsuarioRepository.Obter(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public void Cadastrar(ApplicationUser usuario, string senha)
        {
            var result = _userManager.CreateAsync(usuario, senha).Result;
            if (!result.Succeeded)
            {
                StringBuilder sb = new();
                foreach (var erro in result.Errors)
                {
                    sb.Append(erro.Description);
                }

                throw new Exception($"Usuário não cadadastrado! {sb}");
            }
        }
    }
}
