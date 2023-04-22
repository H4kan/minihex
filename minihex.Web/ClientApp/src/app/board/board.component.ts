import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import Phaser from 'phaser';

@Component({
  selector: 'board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BoardComponent implements OnInit {

  phaserGame: Phaser.Game | undefined;
  config: Phaser.Types.Core.GameConfig;
  constructor() {
    this.config = {
      type: Phaser.AUTO,
      height: 600,
      width: 1000,
      scene: [MainScene],
      parent: 'game-container',
      backgroundColor: '#312e2b'
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

    // Define the size and spacing of the hexagons
    var hexSize = 30;

    // Define the number of rows and columns for the hexagon board
    var numRows = 11;
    var numCols = 11;

    // Create a new graphics object
    var horizontalBorderGraphics = this.add.graphics();

    // Set the fill color and line style for the graphics object
    horizontalBorderGraphics.fillStyle(0xffffff);
    horizontalBorderGraphics.lineStyle(2, 0x312e2b);
    // Draw borders
    for (var row = 0; row < numRows; row++) {
      var [x, y] = this.getHexagonPosition(row, 0, hexSize);
      this.drawHexagon(horizontalBorderGraphics, x - 8, y + 5, hexSize);
    }
    for (var row = 0; row < numRows; row++) {
      var [x, y] = this.getHexagonPosition(row, numCols - 1, hexSize);
      this.drawHexagon(horizontalBorderGraphics, x + 8, y - 1, hexSize);
    }

    // Create a new graphics object
    var verticalBorderGraphics = this.add.graphics();

    // Set the fill color and line style for the graphics object
    verticalBorderGraphics.fillStyle(0x000000);
    verticalBorderGraphics.lineStyle(0, 0x312e2b);
    // Draw borders
    for (var col = 0; col < numCols; col++) {
      var [x, y] = this.getHexagonPosition(0, col, hexSize);
      this.drawHexagon(verticalBorderGraphics, x, y - 7, hexSize);
    }
    for (var col = 0; col < numCols; col++) {
      var [x, y] = this.getHexagonPosition(numRows - 1, col, hexSize);
      this.drawHexagon(verticalBorderGraphics, x, y + 10, hexSize);
    }


    // Loop through each row and column to create the hexagon tiles
    for (var row = 0; row < numRows; row++) {
      for (var col = 0; col < numCols; col++) {
        // Calculate the position of the current hexagon tile
        var [x, y] = this.getHexagonPosition(row, col, hexSize);
        // Draw the hexagon tile

        var points = [];
        for (var i = 0; i < 6; i++) {
          var angle = 2 * Math.PI / 6 * (i + 0.5);
          var px = hexSize * Math.cos(angle);
          var py = hexSize * Math.sin(angle);
          points.push(px, py);
        }
        var hexagon = this.add.polygon(-4 + hexSize + x, 2 + hexSize + y, points, 0xbbbbbb, 1) as Hexagon;

        // make shape interactive
        hexagon.setInteractive(new Phaser.Geom.Polygon(points), Phaser.Geom.Polygon.Contains);

        hexagon.row = row;
        hexagon.col = col;
        hexagon.inUse = false;
        // add click event listener
        hexagon.on('pointerdown', function (this: Hexagon) {
          if (!this.inUse) {
            this.scene.add.circle(this.x - hexSize + 3, this.y - hexSize, hexSize / 2, 0x000000);
            this.inUse = true;
          }
        });
        hexagon.on('pointerover', function (this: Hexagon) {
          if (!this.inUse) {
            this.fillColor = 0xdddddd;
          }
        });
        hexagon.on('pointerout', function (this: Hexagon) {
          if (!this.inUse) {
            this.fillColor = 0xbbbbbb;
          }
        });
      }
    }

    // Draw labels
    for (var row = 0; row < numRows; row++) {
     this.add.text(2 * row + (1.2 + 1.8 * row) * hexSize, 0, String.fromCharCode(65 + row),
        { fixedWidth: 2 * hexSize, align: 'center', color: '#5CDB95' });

      this.add.text(0.3 * hexSize + row * hexSize, 2* row + 1.7 * hexSize + (1.5 * row) * hexSize, `${row + 1}`,
        { align: 'center', color: '#5CDB95' });
    }
  }

  preload() {
    //console.log('preload method');
  }
  update() {
    //console.log('update method');
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

  getHexagonPosition(i: number, j: number, hexSize: number) {
    return [2 * j + 1.2 * hexSize + i * hexSize + (1.8 * j + 1) * hexSize, 2 * i + 25 + (1.5 * i + 1) * hexSize];
  }


}

interface Hexagon extends Phaser.GameObjects.Polygon {
  row: number;
  col: number;
  inUse: boolean;
}



