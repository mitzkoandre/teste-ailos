namespace Questao2
{
    public class MatchService
    {
        private readonly MatchClient _matchClient;

        public MatchService()
        {
            _matchClient = new MatchClient();
        }

        public async Task<int> GetTotalScoredGoalsAsync(string team, int year)
        {
            int totalGoals = 0;

            MatchResponse dataTeam1 = await GetFootballDataAsync(team, year, true);
            totalGoals += CalculateTotalGoals(dataTeam1, true);

            MatchResponse dataTeam2 = await GetFootballDataAsync(team, year, false);
            totalGoals += CalculateTotalGoals(dataTeam2, false);

            return totalGoals;
        }

        private async Task<MatchResponse> GetFootballDataAsync(string team, int year, bool isTeam1)
        {
            MatchResponse response = null;

            response = await _matchClient.GetMatchesAsync(year, team, 1, isTeam1);

            if (response == null)
                return null;

            var tasks = new List<Task<MatchResponse>>();

            for (int i = 2; i <= response.TotalPages; i++)
                tasks.Add(_matchClient.GetMatchesAsync(year, team, i, isTeam1));

            var results = await Task.WhenAll(tasks);

            foreach (var matchResponse in results)
            {
                if (matchResponse?.Data != null)
                    response.Data.AddRange(matchResponse.Data);
            }

            return response;
        }

        private int CalculateTotalGoals(MatchResponse matches, bool isTeam1)
        {
            return matches.Data.Sum(m => isTeam1 ? int.Parse(m.Team1Goals) : int.Parse(m.Team2Goals));
        }
    }
}
