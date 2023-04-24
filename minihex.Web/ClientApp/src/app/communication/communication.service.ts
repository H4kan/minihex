import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommunicationService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl + "game/";
  }

  // call server to start game with some setup
  // returns some game id
  beginGame(request: UserSettings): Promise<string> {



    return new Promise((resolve) => {
      this.http.post<GameIdentificator>(this.baseUrl + "beginGame", request)
        .subscribe((gameIdentificator: GameIdentificator) => {
          resolve(gameIdentificator.gameId);
        })
    });
  }

  // get next move in some position
  // returns index of field on board
  getNextMove(gameId: string, moveNumber: number): Promise<NextMoveInfo> {

    let request: GetMoveRequest = {
      gameId,
      moveNumber
    }

    return new Promise((resolve) => {
      this.http.post<NextMoveInfo>(this.baseUrl + "getMove", request)
        .subscribe((response: NextMoveInfo) => {
          resolve(response);
        })
    });
  }

  // informs server about move made
  makeNextMove(gameId: string, fieldIdx: number, moveNumber: number): Promise<NextMoveInfo> {

    let request: MakeMoveRequest = {
      gameId,
      moveNumber,
      fieldIdx
    }

    return new Promise((resolve) => {
      this.http.post<NextMoveInfo>(this.baseUrl + "makeMove", request)
        .subscribe((response: NextMoveInfo) => {
          resolve(response);
        })
    });
  }

  // after game ends
  // should return who won and winning path
  getWinningPath(gameId: string): Promise<WinningPathInfo> {

    let request: GameIdentificator = {
      gameId
    }

    return new Promise((resolve) => {
      this.http.post<WinningPathInfo>(this.baseUrl + "getWinningPath", request)
        .subscribe((response: WinningPathInfo) => {
          resolve(response);
        })
    });
  }
}

export interface NextMoveInfo {
  fieldIdx: number;
  status: GameStatus,
  gameId: string
}

export interface WinningPathInfo {
  colorWon: PlayerColor,
  path: number[],
  gameId: string
}

export enum GameStatus {
  InProgress = 0,
  Finished = 1
}

export enum PlayerColor {
  White = 0,
  Black = 1
}

export interface UserSettings {

  player1Variant: Algorithm;

  player2Variant: Algorithm;

  variant: GameVariant;

  size: number;

  delay: number;

}

export enum Algorithm {

  Human = 0,
  Heuristic = 1,
  MCTS = 2,
  MSTSwAMAF = 3,
  MCTSwSavebridge = 4,
  MCTSwAMAFandSavebridge = 5

}

export enum GameVariant {

  Basic = 0,
  SWAP = 1

}

interface GetMoveRequest {

  gameId: string;
  moveNumber: number;

}

interface MakeMoveRequest {

  gameId: string;
  moveNumber: number;
  fieldIdx: number;
}

interface GameIdentificator {
  gameId: string;
}
