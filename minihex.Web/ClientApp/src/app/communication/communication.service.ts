import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommunicationService {

  // remove this when there is actual logic
  j = 0;

  // call server to start game with some setup
  // returns some game id
  beginGame(request: UserSettings): Promise<string> {
    return new Promise((resolve) => {
      resolve((this.j++) + "");
    });
  }

  // get next move in some position
  // returns index of field on board
  getNextMove(gameId: string, moveNumber: number): Promise<NextMoveInfo> {
    console.log("get" + moveNumber);
    return new Promise((resolve) => {
      resolve({
        fieldIdx: moveNumber,
        status: moveNumber < 5 ? GameStatus.InProgress : GameStatus.Finished,
        gameId
      })
    }); 
  }

  // informs server about move made
  makeNextMove(gameId: string, fieldIdx: number, moveNumber: number): Promise<NextMoveInfo> {
    console.log("make" + moveNumber);
    return new Promise((resolve) => {
      resolve({
        fieldIdx: fieldIdx,
        status: moveNumber < 5? GameStatus.InProgress : GameStatus.Finished,
        gameId
      })
    }); 
  }

  // after game ends
  // should return who won and winning path
  getWinningPath(gameId: string): Promise<WinningPathInfo> {
    return new Promise((resolve) => {
      resolve({
        colorWon: PlayerColor.Black,
        path: [0, 1, 2, 3, 4, 5],
        gameId
      })
    })
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
