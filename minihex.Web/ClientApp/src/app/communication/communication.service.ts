import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommunicationService {

  i = 0;
  // call server to start game with some setup
  // returns some game id
  beginGame(request: BeginGameRequest): number {
    return 0;
  }

  // get next move in some position
  // returns index of field on board
  getNextMove(gameId: number, moveNumber: number): Promise<NextMoveInfo> {
    return new Promise((resolve) => {
      resolve({
        fieldIdx: this.i++,
        status: this.i < 5 ? GameStatus.InProgress : GameStatus.Finished
      })
    }); 
  }

  // informs server about move made
  makeNextMove(gameId: number, fieldIdx: number, moveNumber: number): Promise<NextMoveInfo> {
    return new Promise((resolve) => {
      resolve({
        fieldIdx: fieldIdx,
        status: this.i < 5 ? GameStatus.InProgress : GameStatus.Finished
      })
    }); 
  }

  // after game ends
  // should return who won and winning path
  getWinningPath(gameId: number): Promise<WinningPathInfo> {
    return new Promise((resolve) => {
      resolve({
        colorWon: PlayerColor.Black,
        path: [0, 1, 2, 3, 4, 5]
      })
    })
  }
}


// TODO: fill this
interface BeginGameRequest {
  variant: AlgorithmVariant;
}

export interface NextMoveInfo {
  fieldIdx: number;
  status: GameStatus
}

export interface WinningPathInfo {
  colorWon: PlayerColor,
  path: number[]
}

export enum GameStatus {
  InProgress = 0,
  Finished = 1
}

export enum PlayerColor {
  White = 0,
  Black = 1
}

enum AlgorithmVariant {
  Heuristic = 1,
  MCTS = 2,
  AMAF = 3,
  Savebridge = 4,
  AMAFSavebridge = 5
}
