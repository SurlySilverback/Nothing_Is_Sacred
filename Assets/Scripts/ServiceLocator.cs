public class ServiceLocator : Singleton<ServiceLocator>
{
    private Player player;
    private InfoView viewInfo;
    private MapGraph mapGraph;
    private MainGovernment mainGov;
    private InGameTime clock;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        viewInfo = FindObjectOfType<InfoView>();
        mapGraph = FindObjectOfType<MapGraph>();
        mainGov = FindObjectOfType<MainGovernment>();
        clock = FindObjectOfType<InGameTime>();
    }

    public InGameTime GetClock()
    {
        return clock;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public InfoView GetViewInfo()
    {
        return viewInfo;
    }

    public MapGraph GetMapGraph()
    {
        return mapGraph;
    }

    public MainGovernment GetMainGovernment()
    {
        return mainGov;
    }
}