using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using System.Net.Http;
using System.Text.Json;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services
{
    public class EnderecoService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepository<Endereco> _enderecoRepository;

        public EnderecoService(IHttpClientFactory httpClientFactory, IRepository<Endereco> enderecoRepository)
        {
            _httpClientFactory = httpClientFactory;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<Endereco> GetEnderecoDto(ClienteRequest clienteRequest)
        {
            var cep = clienteRequest.CEP.ToString();
            var numero = clienteRequest.Numero.ToString();
            EnderecoDTO enderecoDto = await GetEnderecoFromApi(cep);

            if (_enderecoRepository is EnderecoRepository enderecoRepository)
            {
                var endereco = await enderecoRepository.GetByCEPAsync(cep);

                if (endereco != null)
                {

                    bool comparacao = await CompareEndereco(endereco, enderecoDto);

                    if (comparacao && endereco.Numero == numero)
                    {
                        return endereco;
                    }
                }

                endereco = await TransformarDTO(enderecoDto, numero);

                if (endereco != null)
                {
                    await enderecoRepository.AddAsync(endereco);
                    return endereco;
                }
                else
                {
                    return null;
                }

            } else
            {
                throw new Exception("EnderecoRepository não é do tipo EnderecoRepository");
            }
            
        }

        private async Task<EnderecoDTO> GetEnderecoFromApi(string cep)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://brasilapi.com.br/api/cep/v1/{cep}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var enderecoDto = JsonSerializer.Deserialize<EnderecoDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return enderecoDto;
        }

        private async Task<bool> CompareEndereco (Endereco endereco, EnderecoDTO enderecoDTO)
        {
            return await Task.Run(() => endereco.Rua == enderecoDTO.Rua && endereco.Bairro == enderecoDTO.Bairro && endereco.Cidade == enderecoDTO.Cidade && endereco.Estado == enderecoDTO.Estado && endereco.CEP == enderecoDTO.CEP);
        }

        private async Task<Endereco> TransformarDTO (EnderecoDTO enderecoDTO, string numero)
        {
            return await Task.Run(() => new Endereco
            {
                Rua = enderecoDTO.Rua,
                Bairro = enderecoDTO.Bairro,
                Cidade = enderecoDTO.Cidade,
                Estado = enderecoDTO.Estado,
                CEP = enderecoDTO.CEP,
                Numero = numero
            });
        }
    }
}
