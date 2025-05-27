using UnityEngine;

public class GameOver : MonoBehaviour
{

    private PuzzleSaver puzzle;
    private HudVidaController hudController;
    private PuzzleProgressManager puzzleProgressManager;
    private void Awake()
    {
        puzzle = PuzzleSaver.Instance;
        hudController = HudVidaController.Instance;

        puzzle.puzzle1_sala1 = false;
        puzzle.puzzle1_sala2 = false;
        puzzle.puzzle2_sala2 = false;
        puzzle.puzzle1_sala3 = false;
        puzzle.puzzle2_sala3 = false;
        puzzle.puzzle2_sala4 = false;
        puzzle.puzzle1_sala4 = false;
        puzzle.puzzle3_sala4 = false;
        puzzle.puzzle1_salaSecreta = false;

        puzzle.entrouSalaSecreta = false;

        puzzle.puzzle1_sala5 = false;

        hudController.ResetarVidas();
        PuzzleProgressManager.Instance.ResetProgress();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
