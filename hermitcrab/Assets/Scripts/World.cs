using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    public enum Sound {
        AngrySeagull,
        CantGrab,
        GameOverSwoop,
        SeagullScreech,
        ShellGrab,
        ShellPop,
    }

    [SerializeField] AudioSource music1;
    [SerializeField] AudioSource ambiance;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource sfxPop;
    [SerializeField] AudioSource walking;
    [SerializeField] AudioSource seagulls;

    [SerializeField] AudioClip NoShell1;
    [SerializeField] AudioClip NoShell2;
    [SerializeField] AudioClip OpeningCutscene;
    [SerializeField] AudioClip Shell1;
    [SerializeField] AudioClip Shell2;
    [SerializeField] AudioClip Shell3;
    [SerializeField] AudioClip LastShellObtained;
    [SerializeField] AudioClip Win;

    [SerializeField] AudioClip AngrySeagull;
    [SerializeField] AudioClip CantGrab;
    [SerializeField] AudioClip GameOverSwoop;
    [SerializeField] AudioClip Ocean;
    [SerializeField] AudioClip SeagullScreech;
    [SerializeField] AudioClip ShellGrab;
    [SerializeField] AudioClip TitleScreenAmbiance;
    [SerializeField] AudioClip Walking;
    [SerializeField] AudioClip ShellPop;

    [SerializeField] Crab crab;
    [SerializeField] HUD hud;

    public static void PlaySound(Sound sound) {
        instance.PlaySoundInst(sound);
    }

    void PlaySoundInst(Sound sound) {
        AudioClip clip = null;
        switch (sound) {
            case (Sound.AngrySeagull): clip = NoShell1; break;
            case (Sound.CantGrab): clip = CantGrab; break;
            case (Sound.GameOverSwoop): clip = GameOverSwoop; break;
            case (Sound.SeagullScreech): clip = SeagullScreech; break;
            case (Sound.ShellGrab): clip = ShellGrab; break;
            case (Sound.ShellPop): clip = ShellPop; break;
        }

        AudioSource src = sfx;
        if (sound == Sound.ShellPop) {
            src = sfxPop;
        }

        src.PlayOneShot(clip);
    }

    public static void SetWalking(bool w) {
        if (w && !instance.walking.isPlaying) {
            instance.walking.Play();
        }
        else if (!w) {
            instance.walking.Stop();
        }
    }

    public static void PlayLastShellMusic() {
        instance.music1.clip = instance.LastShellObtained;
        instance.music1.Play();
    }

    static World instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        music1.Stop();
        ambiance.Stop();
        walking.Stop();
        seagulls.Stop();

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Title") {
            ambiance.clip = TitleScreenAmbiance;
            ambiance.Play();
        }
        else {
            ambiance.clip = Ocean;
            ambiance.Play();
        }

        if (sceneName == "GenScene") {
            music1.clip = NoShell1;
            music1.Play();
        }

        if (sceneName == "Win") {
            music1.clip = Win;
            music1.Play();
        }

        if (sceneName == "GenScene" || sceneName == "Title") {
            seagulls.clip = AngrySeagull;
            seagulls.Play();
        }

        crab = FindObjectOfType<Crab>();
        hud = FindObjectOfType<HUD>();
    }

    void Update() {
        string sceneName = SceneManager.GetActiveScene().name;

        if (Input.GetButtonDown("Cancel")) {
            SceneManager.LoadScene("Title");
        }

        if (sceneName == "GenScene") {
            if (!music1.isPlaying) {
                if (crab.shell == null) {
                    if (crab.sizeIndex == 0 || crab.sizeIndex == 2 || crab.sizeIndex == 4) {
                        music1.clip = NoShell1;
                    }
                    else {
                        music1.clip = NoShell2;
                    }
                }
                else {
                    if (crab.sizeIndex == 0 || crab.sizeIndex == 3) {
                        music1.clip = Shell1;
                    }
                    else if (crab.sizeIndex == 1 || crab.sizeIndex == 4) {
                        music1.clip = Shell2;
                    }
                    else {
                        music1.clip = Shell3;
                    }
                }
                music1.Play();
            }

            hud.SetSize(crab.periodicGrowTime / crab.growPeriod);

            if (crab.shell == null) {
                hud.SetText("Find a home!");
            }
            else {
                hud.SetText("Grow!");
            }
        }
    }
}
