import struct, zlib, math, os
SIZE = 48
cx = cy = SIZE / 2
pixels = bytearray(SIZE * SIZE * 4)
outerR = 21; innerR = 15; centerR = 7; numTeeth = 8
toothHalfAngle = math.pi / numTeeth * 0.55
for y in range(SIZE):
    for x in range(SIZE):
        dx = x + 0.5 - cx; dy = y + 0.5 - cy
        dist = math.sqrt(dx*dx + dy*dy)
        angle = math.atan2(dy, dx)
        if dist <= centerR: continue
        inside = False
        if dist <= innerR: inside = True
        elif dist <= outerR:
            for i in range(numTeeth):
                ta = (2*math.pi*i)/numTeeth
                diff = angle - ta
                while diff > math.pi: diff -= 2*math.pi
                while diff < -math.pi: diff += 2*math.pi
                if abs(diff) <= toothHalfAngle: inside = True; break
        if inside:
            idx = (y*SIZE+x)*4
            pixels[idx]=255; pixels[idx+1]=255; pixels[idx+2]=255; pixels[idx+3]=255
def make_chunk(ct, d):
    chunk = ct + d
    return struct.pack('>I', len(d)) + chunk + struct.pack('>I', zlib.crc32(chunk) & 0xffffffff)
sig = b'\x89PNG\r\n\x1a\n'
ihdr = struct.pack('>IIBBBBB', SIZE, SIZE, 8, 6, 0, 0, 0)
raw = bytearray()
for y in range(SIZE):
    raw.append(0)
    for x in range(SIZE):
        idx = (y*SIZE+x)*4; raw.extend(pixels[idx:idx+4])
comp = zlib.compress(bytes(raw))
png = sig + make_chunk(b'IHDR', ihdr) + make_chunk(b'IDAT', comp) + make_chunk(b'IEND', b'')
path = '/Users/kubaflo/Desktop/AlohaAI/src/AlohaAI/Resources/Images/icon_settings.png'
os.makedirs(os.path.dirname(path), exist_ok=True)
with open(path, 'wb') as f: f.write(png)
print('Done:', len(png), 'bytes')
