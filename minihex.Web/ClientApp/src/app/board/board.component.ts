import { Component, OnInit } from '@angular/core';
import Phaser from 'phaser';

@Component({
  selector: 'board',
  templateUrl: './board.component.html',
})
export class BoardComponent implements OnInit {

  phaserGame: Phaser.Game | undefined;
  config: Phaser.Types.Core.GameConfig;
  constructor() {
    this.config = {
      type: Phaser.AUTO,
      height: 600,
      width: 800,
      scene: [MainScene]
    };
  }
  ngOnInit(): void {
    this.phaserGame = new Phaser.Game(this.config);
  }
}

class MainScene extends Phaser.Scene {
  constructor() {
    super({ key: 'main' });
  }
  create() {
    // Create a new graphics object
    var graphics = this.add.graphics();

    // Set the fill color and line style for the graphics object
    graphics.fillStyle(0xffffff);
    graphics.lineStyle(2, 0x000000);

    // Define the size and spacing of the hexagons
    var hexSize = 50;
    var hexSpacing = 3;

    // Define the number of rows and columns for the hexagon board
    var numRows = 10;
    var numCols = 10;

    // Loop through each row and column to create the hexagon tiles
    //for (var row = 0; row < numRows; row++) {
    //  for (var col = 0; col < numCols; col++) {
    //    // Calculate the position of the current hexagon tile
    //    //var x = hexSpacing * Math.sqrt(3) * (col + 0.5 * (row % 2));
    //    //var y = hexSpacing * 1.5 * row;
    //    var x = col * 100;
    //    var y = row * 100;

    //    // Draw the hexagon tile
    //    this.drawHexagon(graphics, x, y, hexSize);
    //  }
    //}
    this.drawHexagon(graphics, 0, 0, hexSize);
  }

  preload() {
    console.log('preload method');
  }
  update() {
    console.log('update method');
  }

  // Draw a hexagon shape using the graphics object
   drawHexagon(graphics: Phaser.GameObjects.Graphics, x: number, y: number, size: number) {
     // Calculate the points of the hexagon shape
     var points = [];
     for (var i = 0; i < 6; i++) {
       var angle = 2 * Math.PI / 6 * (i + 0.5);
       var px = x + size * Math.cos(angle);
       var py = y + size * Math.sin(angle);
       points.push(px, py);
     }

     // Draw the hexagon shape using the graphics object
     graphics.beginPath();
     graphics.moveTo(points[0], points[1]);
     for (var i = 2; i < points.length - 1; i += 2) {
       graphics.lineTo(points[i], points[i + 1]);
     }
     graphics.closePath();
     graphics.fillPath();
     graphics.strokePath();
  }
}



