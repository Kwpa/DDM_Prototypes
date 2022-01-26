using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerBotManager : MonoBehaviour
{
    public List<Player> _generatedPlayers = new List<Player>();
    public int _genSize = 0;
    public List<int> _actionPointAllocations = new List<int>() {4,5};
    GameManager _gMgr;

    public List<string> _usernames = new List<string>(){ "blantsargue", "instructorcosty", "carpentergillette", "luminouspipeweed", "parsleycautious", "flenchstore", "healthymullins", "rodolphmangoes", "mountmell", "eightyquietly", "risottorustica", "spinolaeeffluvium", "spreadproduct", "fastenguitar", "nairobiforemast", "longaristotelis", "fliskssample", "woodscoop", "residentmay", "reindeerpallograph", "narnishserrirostris", "ochotonafuga", "zoonderkinsscrap", "quaaltaghthumbsdown", "salfordeuropaeus", "felinetotalus", "covetloxahatchee", "pipchinadmired", "beepybaryphyma", "impossiblemoria", "toucancroe", "housewoots", "junkyixobrychus", "jarenhay", "atroflavahassel", "nitrogenlinhope", "egfordgadolinium", "travelermatutolypea", "bothlongifolia", "howickdodgeball", "fizkinaccess", "wootshealth", "sharpcomet", "hornseybreeding", "forgetfulwetsuit", "typicalvehicle", "paludosauncertain", "serpentinefraid", "macrocephalaholy", "biohazardflig", "pourcaring", "soldierclyde", "boreheadtraligill", "motheramalia", "crapulouspocket", "sanguinemecopisthes", "beefimperio", "overworldamon", "reubenpass", "moshgracilis", "enterpriseplunk", "tagespinicola", "spikesplastic", "crapulentcomplex", "pisicrawn", "incurvatumspumps", "majesticfobly", "berserkquiche", "askghast", "wryalvine", "adventillusion", "frequentpriscilla", "dewslitherum", "gorgedzhengzhou", "quiltcarneolutea", "sousaphonepurr", "otherdomesticus", "borealissetaceous", "suitstrudel", "platejukebox", "clipsdolphin", "cardellinahewn", "moneywhispering", "spustairplane", "tavernershimy", "radicalwolverhampton", "norboindiscreet", "bildbeauty", "frugilegusneomycin", "malkinrodge", "quagswagdroning", "koalalipsothrix", "troglodytecreamy", "acaulonherbert", "agilisclooves", "homestoat", "trelawneyjotten", "loyaltymelanoxeros", "vuffinfulgens", "diarsiamocking", "proposeborder", "existencehat", "smuetrounce", "phyteumaalve", "kosingmough", "cellsplume", "arklesamarium", "opossummann", "individualrange", "overallscells", "compressusboatswain", "googlenunney", "vouchgaywood", "weightspair", "griseawhirlwind", "chinebag", "winchesterjamesoniella", "defiantought", "portlandneodymium" };

    public void Init()
    {
        _gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        GeneratePlayers(_genSize);
    }

    public void GeneratePlayers(int amount)
    {
        List<string> copyUsernames = new List<string>();
        copyUsernames.AddRange(_usernames);
        for(int i=0; i< amount; i++)
        {
            string id = "player_" + i;
            string name = copyUsernames[Random.Range(0, copyUsernames.Count - 1)];
            int actionPoints = AllocateActionPoints(i);
            Player player = new Player(id, name, actionPoints, _gMgr._teams);
            player._playerIsBot = true;
            player._playerToTeamData = GiveBotLikeDislikeBehaviour(player);
            
            _generatedPlayers.Add(player);
        }
    }

    public int AllocateActionPoints(int id)
    {
        //for each element 
        for(int i=0; i<_actionPointAllocations.Count;i++)
        {
            //if we are
            if ((id+1) % (i+1) == 0)
            {
                return _actionPointAllocations[i];
            }
        }
        return 0;
    }

    public void BotsPerformActions()
    {
        foreach(KeyValuePair<string,Player> kvp in _gMgr._players)
        {
            print("bots");
            Player player = kvp.Value;
            if(player._playerIsBot)
            {
                print("bot");
                Player bot = player;
                List<string> _teamFavs = new List<string>();
                foreach (KeyValuePair<string, PlayerToTeamData> kvp2 in bot._playerToTeamData.OrderByDescending(p => p.Value._botLikeDislike))
                {
                    string favTeamID = kvp2.Key;
                    print("team " + favTeamID);
                    while(_gMgr._teams[favTeamID]._donationNeeded > 0 || _gMgr._players[bot._playerID]._actionPoints > 0)
                    {
                        _gMgr.SpendActionOnHealthDonation(bot._playerID, favTeamID);
                    }
                }
            }
        }

    }

    public Dictionary<string, PlayerToTeamData> GiveBotLikeDislikeBehaviour(Player p)
    {
        Dictionary<string, PlayerToTeamData> _dict = new Dictionary<string, PlayerToTeamData>();
        foreach(KeyValuePair<string,PlayerToTeamData> kvp in p._playerToTeamData)
        {
            PlayerToTeamData pttd = kvp.Value;
            pttd._botLikeDislike = Random.Range(0, 10);
            _dict.Add(kvp.Key, pttd);
        }
        return _dict;
    }
}
