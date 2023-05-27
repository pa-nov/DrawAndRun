using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Material PlayerColor;
    public GameObject DrawPanel, LevelCompleteWindow, GameOverWindow, DestroyEffect;
    public GameObject[] DefaultCharacters;

    private GameObject[] Characters;
    private int Players = 0, PlayersWin = 0;
    private bool CanMove = false;


    private void Start()
    {
        Instance = this;
        Characters = DefaultCharacters;
        Players = Characters.Length;
    }
    private void FixedUpdate()
    {
        if (CanMove)
        {
            this.transform.position += new Vector3(0, 0, 0.1f);
        }
        Camera.main.transform.position = this.transform.position + new Vector3(0, 10, -15);
    }

    public void SpawnCharacter(GameObject Character)
    {
        Players++;
        Array.Resize(ref Characters, Characters.Length + 1);
        Characters[^1] = Character;

        Character.tag = "Character";
        Character.transform.parent = this.transform;
        Character.GetComponent<Character>().SkinnedMeshRenderer.material = PlayerColor;
        Character.GetComponent<Character>().NewPositon = Character.transform.localPosition;
        Character.GetComponent<Character>().Animator.Play("Running");
    }
    public void DestroyCharacter(GameObject Character)
    {
        GameObject[] NewCharacters = new GameObject[Characters.Length - 1];
        int LastIndex = 0;

        foreach(GameObject character in Characters)
        {
            if (character != Character)
            {
                NewCharacters[LastIndex] = character;
                LastIndex++;
            }
            else
            {
                GameObject Effect = Instantiate(DestroyEffect, character.transform.position, character.transform.rotation);
                Destroy(character);
                Players--;
            }
        }

        Characters = NewCharacters;

        if (Players < 1)
        {
            GameOver();
        }
    }
    public void MoveCharacters(List<Vector2> Path, Vector2 LeftDown, Vector2 RightTop)
    {
        CanMove = true;
        if (Path.Count > 1)
        {
            float PathLength = 0;
            for (int i = 1; i < Path.Count; i++)
            {
                PathLength += Vector3.Magnitude(Path[i] - Path[i - 1]);
            }
            float CharactersDistance = PathLength / Characters.Length;
            float CharactersDistanceSource = CharactersDistance;
            CharactersDistance /= 2;

            int LastPoint = 0;
            for (int i = 0; i < Characters.Length; i++)
            {
                bool Completed = false;
                float PreCurrent = 0, Current = 0;

                while (!Completed)
                {
                    if (Current < CharactersDistance && Path.Count > LastPoint + 1)
                    {
                        PreCurrent = Current;
                        LastPoint++;
                        Current += Vector3.Magnitude(Path[LastPoint] - Path[LastPoint - 1]);
                    }
                    else
                    {
                        LastPoint--;
                        Vector2 NewPoint = Path[LastPoint] +
                            Vector2.ClampMagnitude(Path[LastPoint + 1] - Path[LastPoint], CharactersDistance - PreCurrent);
                        Path[LastPoint] = NewPoint;
                        Characters[i].GetComponent<Character>().NewPositon = new Vector3(
                            GetWorldPosition(NewPoint, LeftDown, RightTop).x * 4.5f,
                            0,
                            GetWorldPosition(NewPoint, LeftDown, RightTop).y * 3);
                        Completed = true;
                    }
                }
                CharactersDistance = CharactersDistanceSource;
            }
        }
        else
        {
            foreach(GameObject Character in Characters)
            {
                Character.GetComponent<Character>().NewPositon = new Vector3(
                            GetWorldPosition(Path[0], LeftDown, RightTop).x * 6,
                            0,
                            GetWorldPosition(Path[0], LeftDown, RightTop).y * 4);
            }
        }
    }

    private void GameOver()
    {
        DrawPanel.SetActive(false);
        GameOverWindow.SetActive(true);
    }
    public void LevelComplete()
    {
        DrawPanel.SetActive(false);
        LevelCompleteWindow.SetActive(true);
        PlayersWin++;
        if (PlayersWin >= Players)
        {
            CanMove = false;
        }
    }

    private Vector2 GetWorldPosition(Vector2 NewPoint, Vector2 LeftDown, Vector2 RightTop)
    {
        NewPoint -= LeftDown;
        RightTop -= LeftDown;
        NewPoint /= RightTop;
        NewPoint = NewPoint * 2 - new Vector2(1, 1);

        return NewPoint;
    }
}