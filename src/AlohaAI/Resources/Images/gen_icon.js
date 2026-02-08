const fs = require('fs');
const zlib = require('zlib');
const SIZE = 48;
const cx = SIZE/2, cy = SIZE/2;
const pixels = Buffer.alloc(SIZE*SIZE*4);
const outerR=21, innerR=15, centerR=7, numTeeth=8;
const toothHalfAngle = Math.PI/numTeeth*0.55;
for(let y=0;y<SIZE;y++){for(let x=0;x<SIZE;x++){
  const dx=x+0.5-cx,dy=y+0.5-cy;
  const dist=Math.sqrt(dx*dx+dy*dy);
  const angle=Math.atan2(dy,dx);
  if(dist<=centerR)continue;
  let inside=false;
  if(dist<=innerR)inside=true;
  else if(dist<=outerR){for(let i=0;i<numTeeth;i++){
    const ta=(2*Math.PI*i)/numTeeth;
    let diff=angle-ta;
    while(diff>Math.PI)diff-=2*Math.PI;
    while(diff<-Math.PI)diff+=2*Math.PI;
    if(Math.abs(diff)<=toothHalfAngle){inside=true;break;}
  }}
  if(inside){const idx=(y*SIZE+x)*4;pixels[idx]=255;pixels[idx+1]=255;pixels[idx+2]=255;pixels[idx+3]=255;}
}}
function crc32(buf){let c=0xFFFFFFFF;for(let i=0;i<buf.length;i++){c^=buf[i];for(let j=0;j<8;j++)c=c&1?0xEDB88320^(c>>>1):c>>>1;}return(c^0xFFFFFFFF)>>>0;}
function makeChunk(type,data){const td=Buffer.concat([type,data]);const len=Buffer.alloc(4);len.writeUInt32BE(data.length);const crc=Buffer.alloc(4);crc.writeUInt32BE(crc32(td)>>>0);return Buffer.concat([len,td,crc]);}
const sig=Buffer.from([0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A]);
const ihdr=Buffer.alloc(13);ihdr.writeUInt32BE(SIZE,0);ihdr.writeUInt32BE(SIZE,4);ihdr[8]=8;ihdr[9]=6;
const raw=[];for(let y=0;y<SIZE;y++){raw.push(0);for(let x=0;x<SIZE;x++){const idx=(y*SIZE+x)*4;raw.push(pixels[idx],pixels[idx+1],pixels[idx+2],pixels[idx+3]);}}
const comp=zlib.deflateSync(Buffer.from(raw));
const png=Buffer.concat([sig,makeChunk(Buffer.from('IHDR'),ihdr),makeChunk(Buffer.from('IDAT'),comp),makeChunk(Buffer.from('IEND'),Buffer.alloc(0))]);
fs.writeFileSync('/Users/kubaflo/Desktop/AlohaAI/src/AlohaAI/Resources/Images/icon_settings.png',png);
console.log('Done:',png.length,'bytes written');
console.log('Base64:',png.toString('base64'));
