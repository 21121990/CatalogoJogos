﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Exceptions;
using WebApplication1.InputModel;
using WebApplication1.Respositories;
using WebApplication1.ViewModel;

namespace WebApplication1.Services
{
    public class JogoService : IjogoService
    {
        private readonly IjogoRepository _jogoRepository;

        public JogoService(IjogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoViewModel>> Obter (int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);
            return jogos.Select(jogo => new JogoViewModel
            {
                id = jogo.id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }
        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
            {
                return null;
            }
            return new JogoViewModel
            {
                id = jogo.id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }
        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)
            {
                throw new JogoJaCadastradoException();
            }
            var jogoInsert = new Jogo {
                id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {

                id = jogoInsert.id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco

            };
        }
        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);
            if (entidadeJogo.id == null)
            {
                throw new JogoNaoCadastradoException();
            }
            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);

        }
        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);
            if (entidadeJogo == null)
            {
                throw new JogoNaoCadastradoException();
            }
         
            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);

        }
        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);
            if (jogo== null)
            {
                throw new JogoNaoCadastradoException();
            }
            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }

    }
}
