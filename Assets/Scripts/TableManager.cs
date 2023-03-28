using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour {

    public static TableManager instance;
    public int strikesCount = 0;
    public float minCallTime, maxCallTime;
    public float maxRepetitions = 2;
    public float endedCallTime;
    public float randomCadency;
    public float randomCadencyFinal;
    private float randomCadencyEdit;
    public GameObject timeManager;
    public GameObject gameOverPanel;
    public List<Image> strikeLights;
    public List<PlugSender> plugSenderLetterList;
    public List<PlugSender> plugSenderNumberList;
    private List<Plug> letterPlugs = new List<Plug> ();
    private List<Plug> numberPlugs = new List<Plug> ();
    public List<Call> waitingCalls;
    public List<Connection> tableConnections = new List<Connection> ();

    private Plug currentPlug = new Plug ();
    private int waitingIndex = 0;

    // Singleton inicialization
    private void Awake () {
        Time.timeScale = 1;

        randomCadencyEdit = randomCadency;

        if (instance == null) {
            instance = this;
        } else {
            Destroy (gameObject);
            return;
        }
    }

    void Start () {
        foreach (var item in plugSenderLetterList) {
            letterPlugs.Add (item.plug);
        }

        foreach (var item in plugSenderNumberList) {
            numberPlugs.Add (item.plug);
        }

    }

    private void Update () {
        if (strikesCount == 3) {
            StartCoroutine(GameOver());
        }

        randomCadencyEdit -= Time.deltaTime;

        if (randomCadencyEdit <= 0 && randomCadency >= randomCadencyFinal) {
            randomCadencyEdit = randomCadency;
            randomCadency -= 0.1f;

            CreateRandomCall ();
        }

        if (randomCadency < randomCadencyFinal) {
            randomCadency = randomCadencyFinal;
        }

        foreach (var item in waitingCalls) {
            if (item.repetitions > maxRepetitions) {
                RemoveRandomCall (item);
                TableManager.instance.MakeStrike();

                break;
            }
        }
    }

    public Plug CurrentPlug {
        get {
            return currentPlug;
        }

        set {
            currentPlug = ConnectorsBehavior (value);
        }
    }

    Plug ConnectorsBehavior (Plug _plug) {
        if (string.IsNullOrEmpty (CurrentPlug.plugID)) //Se for a primeira vez que se clica em um Plug;
        {
            if (_plug.plugState == Plug.PlugState.Unplugged) {
                EnableConnector (_plug);
                _plug.plugState = Plug.PlugState.Plugged;

                return _plug;
            } else if (_plug.plugState == Plug.PlugState.Connected) {
                TryCloseConnection (_plug);

                return new Plug ();
            }
        }

        if (CurrentPlug.type == _plug.type && CurrentPlug.plugID != _plug.plugID) //Se vc clicar no mesmo tipo de Plug porém em plugs diferentes;
        {
            if (_plug.plugState == Plug.PlugState.Unplugged && CurrentPlug.plugState == Plug.PlugState.Plugged) {
                EnableConnector (_plug);
                _plug.plugState = Plug.PlugState.Plugged;

                DisableConnector (CurrentPlug);
                CurrentPlug.plugState = Plug.PlugState.Unplugged;

                return _plug;
            }

            return CurrentPlug;
        } else if (CurrentPlug.type == _plug.type && CurrentPlug.plugID == _plug.plugID) //Se vc clicar no mesmo Plug consecutivamente;
        {
            DisableConnector (_plug);
            _plug.plugState = Plug.PlugState.Unplugged;

            return new Plug ();
        } else //Se você clica em plugs de diferentes tipos respectivamente;
        {
            EnableConnector (_plug);

            if (_plug.plugState == Plug.PlugState.Unplugged) {
                _plug.plugState = Plug.PlugState.Plugged;

                TryMakeConnection (CurrentPlug, _plug);
            }
            else
            {
                DisableConnector(currentPlug);
                currentPlug.plugState = Plug.PlugState.Unplugged;

                TableManager.instance.MakeStrike();   
            }
            
            return new Plug ();
        }
    }

    void EnableConnector (Plug _plug) {
        _plug.connector.enabled = true;
    }

    void DisableConnector (Plug _plug) {
        _plug.connector.enabled = false;
    }

    void TryMakeConnection (Plug _plug1, Plug _plug2) {
        bool connectionFound = false;

        // Faz a conexão caso o estado dos dois botões esteja Plugged e se eles forem waitingCalls;
        if (_plug1.plugState == Plug.PlugState.Plugged && _plug2.plugState == Plug.PlugState.Plugged) {
            foreach (var call in waitingCalls) {
                if ((call.plug1 == _plug1 || call.plug1 == _plug2) && (call.plug2 == _plug1 || call.plug2 == _plug2)) {
                    waitingCalls.Remove (call);

                    _plug1.plugState = Plug.PlugState.Connected;
                    _plug2.plugState = Plug.PlugState.Connected;

                    #region DrawLine
                    LineRenderer lr = _plug1.transform.gameObject.AddComponent<LineRenderer> ();

                    lr.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
                    Color lineColor = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

                    lr.startColor = lineColor;
                    lr.endColor = lineColor;
                    lr.startWidth = 0.15f;
                    lr.SetPosition (0, _plug1.transform.position);
                    lr.SetPosition (1, _plug2.transform.position);
                    #endregion

                    IndividualTimer timer = timeManager.AddComponent<IndividualTimer> ();
                    Connection con = new Connection (_plug1, _plug2, lr, timer);

                    timer.connection = con;
                    tableConnections.Add (con);

                    con.plug1.light.enabled = true;
                    con.plug2.light.enabled = true;

                    connectionFound = true;

                    break;
                }
            }
            //Passar por aqui significa que o jogador levou um strike;
            if (connectionFound == false) {
                _plug1.plugState = Plug.PlugState.Unplugged;
                _plug2.plugState = Plug.PlugState.Unplugged;

                DisableConnector (_plug1);
                DisableConnector (_plug2);

                TableManager.instance.MakeStrike();
            }
        }
    }

    public void TryCloseConnection (Plug _plug) {
        if (_plug.plugState == Plug.PlugState.Connected) {
            foreach (var item in tableConnections) {
                if (_plug == item.plug1 || _plug == item.plug2) {
                    Destroy (item.connectionLine);

                    item.plug1.plugState = Plug.PlugState.Unplugged;
                    item.plug2.plugState = Plug.PlugState.Unplugged;

                    DisableConnector (item.plug1);
                    DisableConnector (item.plug2);

                    item.plug1.light.enabled = false;
                    item.plug2.light.enabled = false;

                    if (item.plug1.type == Plug.Type.Letter) {
                        letterPlugs.Add (item.plug1);
                    } else {
                        numberPlugs.Add (item.plug1);
                    }

                    if (item.plug2.type == Plug.Type.Letter) {
                        letterPlugs.Add (item.plug2);
                    } else {
                        numberPlugs.Add (item.plug2);
                    }

                    if (item.State == Connection.ConnectionState.Call) {
                        TableManager.instance.MakeStrike();
                    }

                    Destroy (item.individualTimer);
                    tableConnections.Remove (item);

                    break;
                }
            }
        }
    }

    public void CreateRandomCall () {
        Plug letter = new Plug ();
        Plug number = new Plug ();

        if (letterPlugs.Count != 0) {
            letter = letterPlugs[Random.Range (0, letterPlugs.Count - 1)];
            number = numberPlugs[Random.Range (0, numberPlugs.Count - 1)];

            letterPlugs.Remove (letter);
            numberPlugs.Remove (number);

            waitingCalls.Add (new Call (letter, number));
        }
    }

    public void RemoveRandomCall (Call call) {
        waitingCalls.Remove (call);

        letterPlugs.Add (call.plug1);
        numberPlugs.Add (call.plug2);
    }

    public void MakeStrike()
    {
        strikeLights[strikesCount].enabled = true;

        strikesCount++;

        if (strikesCount <= 3)
        {
            AudioManager.instance.PlaySound("Error");
        }        
    }

    IEnumerator GameOver(){
        yield return new WaitForSeconds(1.0f);

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
}