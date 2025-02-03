using System.Text;
using System.Text.Json;

namespace Questao2
{
    public class MatchClient
    {
        private readonly HttpClient _client;

        public MatchClient()
        {
            _client = new HttpClient();
        }

        public async Task<MatchResponse> GetMatchesAsync(int year, string team, int page, bool isTeam1)
        {
            try
            {
                string teamParameter = isTeam1 ? "team1" : "team2";

                StringBuilder url = new StringBuilder();

                url.Append("https://jsonmock.hackerrank.com/api/football_matches?");
                url.Append($"year={year}&");
                url.Append($"{teamParameter}={team}&");
                url.Append($"page={page}");

                HttpResponseMessage response = await _client.GetAsync(url.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse data = JsonSerializer.Deserialize<MatchResponse>(json);
                    return data;
                }

                throw new ArgumentException($"Falha ao consultar dados do time {team} no ano {year}.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
