"""Generate a stylized, single-photo-inspired, Unity-ready OBJ toy time-machine coupe.
Units: meters. Y up, Z forward, X right. Requires numpy only.
"""
from __future__ import annotations
import os, math, copy, zipfile, json
from dataclasses import dataclass
import numpy as np

OUT = '/mnt/data/ToyTimeMachine_Unity'
os.makedirs(OUT,exist_ok=True)
MATS = {
'silver_body': ((.57,.60,.61), .16, 20),
'silver_light': ((.70,.72,.73), .19, 24),
'silver_shaded': ((.47,.50,.52), .13, 18),
'brushed_metal': ((.63,.66,.67), .17, 16),
'dark_metal': ((.28,.32,.34), .22, 36),
'window_dark': ((.19,.26,.29), .33, 66),
'window_reflection': ((.31,.40,.43), .36, 72),
'glass_trim': ((.11,.15,.17), .13, 20),
'rubber': ((.067,.070,.075), .08, 12),
'rubber_ridges': ((.099,.10,.104), .08, 12),
'wheel_hub': ((.13,.145,.156), .23, 50),
'wheel_silver': ((.39,.41,.43), .27, 58),
'lamp_front': ((.83,.83,.78), .24, 42),
'lamp_rear': ((.51,.09,.095), .18, 45),
'amber': ((.68,.38,.09), .15, 24),
'cable': ((.42,.40,.36), .23, 35),
'black_line': ((.07,.077,.085), .07, 10),
}

class Mesh:
    def __init__(self):
        self.parts = [] # (part_name,material, [verts], [triangles])
    def tri(self, name, mat, a,b,c, hint=None):
        a,b,c= [np.asarray(x,dtype=float) for x in [a,b,c]]
        n=np.cross(b-a,c-a)
        norm=np.linalg.norm(n)
        if norm<1e-10: return
        if hint is not None and np.dot(n,np.asarray(hint))<0:
            b,c=c,b
        self.parts.append((name,mat, [a,b,c]))
    def poly(self,name,mat,pts,hint=None):
        if len(pts)<3: return
        for i in range(1,len(pts)-1): self.tri(name,mat,pts[0],pts[i],pts[i+1],hint)
    def quad(self,name,mat,p0,p1,p2,p3,hint=None):
        self.tri(name,mat,p0,p1,p2,hint)
        self.tri(name,mat,p0,p2,p3,hint)
    def box(self,name,mat,center,sz):
        x,y,z=center; a,b,c=np.asarray(sz)/2
        P=[(x-a,y-b,z-c),(x+a,y-b,z-c),(x+a,y+b,z-c),(x-a,y+b,z-c),
           (x-a,y-b,z+c),(x+a,y-b,z+c),(x+a,y+b,z+c),(x-a,y+b,z+c)]
        faces=[([0,1,2,3],(0,0,-1)),([4,5,6,7],(0,0,1)),([0,4,7,3],(-1,0,0)),
               ([1,5,6,2],(1,0,0)),([3,2,6,7],(0,1,0)),([0,1,5,4],(0,-1,0))]
        for inds,h in faces:self.poly(name,mat,[P[i] for i in inds],h)
    def tube(self,name,mat,from_pt,to_pt,radius=.015,n=8):
        a,b=np.asarray(from_pt,float),np.asarray(to_pt,float)
        d=b-a
        if np.linalg.norm(d)<1e-7:return
        d/=np.linalg.norm(d)
        u=np.cross(d,[0,1,0]);
        if np.linalg.norm(u)<1e-5: u=np.cross(d,[0,0,1])
        u/=np.linalg.norm(u)
        v=np.cross(d,u)
        circle=[radius*(math.cos(2*math.pi*i/n)*u+math.sin(2*math.pi*i/n)*v) for i in range(n)]
        for i in range(n):
            j=(i+1)%n
            self.quad(name,mat,a+circle[i],a+circle[j],b+circle[j],b+circle[i],hint=circle[i]+circle[j])
        self.poly(name,mat,[a+c for c in circle[::-1]],-d)
        self.poly(name,mat,[b+c for c in circle],d)
    def line(self,name,mat,pts,r=.01,n=7):
        for a,b in zip(pts[:-1],pts[1:]):self.tube(name,mat,a,b,r,n)
    def merge(self,other, transform=(0,0,0), name_prefix=''):
        t=np.asarray(transform,float)
        for name,mat,pts in other.parts:
            self.parts.append((name_prefix+name,mat,[p+t for p in pts]))
    def write(self, path, mtl='ToyTimeMachine.mtl'):
        with open(path,'w') as f:
            f.write('# Stylized photo-inspired toy DeLorean / time-machine coupe\n')
            f.write('# 1 unit = 1 meter | Y-up | +Z forward | X right\n')
            f.write(f'mtllib {mtl}\ns off\n')
            current_part=None;current_mat=None;ix=0
            # Group triangles in the same object even if originally interleaved.
            names=list(dict.fromkeys(p[0] for p in self.parts))
            for name in names:
                sub=[p for p in self.parts if p[0]==name]
                f.write(f'o {name}\n')
                current_mat=None
                for _,mat,tri in sub:
                    if mat!=current_mat:
                        f.write('usemtl '+mat+'\n');current_mat=mat
                    n=np.cross(tri[1]-tri[0],tri[2]-tri[0]);n/=np.linalg.norm(n)
                    for p in tri: f.write('v '+' '.join(f'{a:.6f}' for a in p)+'\n')
                    f.write('vn '+' '.join(f'{a:.6f}' for a in n)+'\n')
                    f.write(f'f {ix+1}//{ix//3+1} {ix+2}//{ix//3+1} {ix+3}//{ix//3+1}\n')
                    ix+=3
        return len(self.parts)

# Each section: z, half-width, side top y, side bottom y
SECTIONS = [(-2.24,.79,.80,.43),(-2.10,.87,.89,.39),(-1.73,.915,.96,.38),
            (-1.30,.92,.99,.36),(-.75,.92,.985,.36),(0,.925,.985,.37),
            (.67,.92,1.01,.37),(1.30,.918,.93,.37),(1.84,.88,.81,.40),
            (2.16,.82,.72,.44),(2.25,.73,.69,.48)]

BODY=Mesh()
# Door geometry is authored in car-space and exported relative to the roof hinges.
DOORS={-1:Mesh(),1:Mesh()}
DOOR_HINGES={-1:(-.325,1.478,-.25),1:(.325,1.478,-.25)}
DOOR_REAR=-.535
DOOR_FRONT=.665

# Side panels are physically notched with curved wheel arches.
def sec(z):
    return [np.interp(z,[s[0] for s in SECTIONS],[s[k] for s in SECTIONS]) for k in (1,2,3)]
def arch_bottom(z, low):
    for wz in (-1.34,1.35):
        dx=z-wz
        r=.42
        if abs(dx)<r:
            low=max(low,.355+math.sqrt(max(0.0,r*r-dx*dx)))
    return low

zs=np.array(sorted(set(np.linspace(-2.24,2.25,200).tolist()+
                       [DOOR_REAR,DOOR_FRONT,-.745,.765])))
for sign in (-1,1):
    for z0,z1 in zip(zs[:-1],zs[1:]):
        w0,t0,b0=sec(z0);w1,t1,b1=sec(z1)
        b0,b1=arch_bottom(z0,b0),arch_bottom(z1,b1)
        for k in range(5):
            lo,hi=k/5,(k+1)/5
            def vertex(z,w,t,b,f):
                y=b*(1-f)+t*f
                x=sign*(w-.014*f+.007*math.sin(f*math.pi))
                return (x,y,z)
            p0,p1=vertex(z0,w0,t0,b0,lo),vertex(z1,w1,t1,b1,lo)
            p2,p3=vertex(z1,w1,t1,b1,hi),vertex(z0,w0,t0,b0,hi)
            mat='silver_body' if k in (1,2,3) else 'silver_shaded' if k==0 else 'silver_light'
            mesh=DOORS[sign] if z0>=DOOR_REAR-1e-7 and z1<=DOOR_FRONT+1e-7 else BODY
            mesh.quad('BodyShell',mat,p0,p1,p2,p3,(sign,0,0))
            if mesh is not BODY:
                # Interior-facing silver-painted lining; doors have actual thickness/readability.
                def inner(pt): return (pt[0]-sign*.040,pt[1],pt[2])
                mesh.quad('DoorInside','silver_shaded',inner(p0),inner(p3),inner(p2),inner(p1),(-sign,0,0))

# Folded top skin with a slightly raised central spine.
xfractions=[-1,-.89,-.68,-.40,0,.40,.68,.89,1]
for z0,z1 in zip(zs[:-1],zs[1:]):
    w0,t0,_=sec(z0);w1,t1,_=sec(z1)
    if z0>=-.745-1e-7 and z1<=.765+1e-7:
        continue  # Open cockpit, not a solid slab under the doors and windscreen.
    for j in range(len(xfractions)-1):
        xa,xb=xfractions[j:j+2]
        def top(xf,w,y,z):
            return (xf*(w-.014),y+.022*(1-xf*xf),z)
        mat='silver_light' if j in (0,7) else 'silver_body'
        BODY.quad('UpperBody',mat,top(xa,w0,t0,z0),top(xb,w0,t0,z0),top(xb,w1,t1,z1),top(xa,w1,t1,z1),(0,1,0))

# Underbody and raised sills, keep wheel clearance.
BODY.box('Undercarriage','dark_metal',(0,.43,0),(1.51,.23,4.16))
for sign in (-1,1):
    BODY.box('SideSills','silver_shaded',(sign*.818,.46,0),(.13,.12,2.0))
    for wz in (-1.34,1.35):
        # raised wheel arch bead matches physical body opening
        arc=[]
        for i in range(25):
            theta=i*math.pi/24
            arc.append((sign*(sec(wz)[0]+.002),.355+.427*math.sin(theta),wz-.427*math.cos(theta)))
        BODY.line('FenderLips','brushed_metal',arc,.014)
        # accent body strips between wheel arches
    BODY.line('BeltLine','silver_shaded',[
        (sign*.919,.92,-1.01),(sign*.935,.955,-.78),(sign*.933,.95,DOOR_REAR-.018)],.010)
    BODY.line('BeltLine','silver_shaded',[
        (sign*.936,.962,DOOR_FRONT+.022),(sign*.924,.91,1.02)],.010)
    DOORS[sign].line('DoorBeltLine','silver_shaded',[
        (sign*.936,.953,DOOR_REAR+.011),(sign*.943,.945,-.05),
        (sign*.938,.96,DOOR_FRONT-.009)],.010)

# Hood creases and inset rectangular detail
for sign in (-1,1):
    def hood_y(z, x=.48):return sec(z)[1]+.022*(1-(x/sec(z)[0])**2)+.011
    BODY.line('HoodCreases','brushed_metal',[(sign*.54,hood_y(z),z) for z in [.94,1.23,1.57,1.92,2.12]],.009)
    BODY.line('HoodCreases','silver_shaded',[(sign*.73,hood_y(z,.73),z) for z in [1.06,1.45,1.82,2.05]],.010)
BODY.line('HoodEdge','silver_shaded',[(-.72,.747,2.11),(0,.74,2.11),(.72,.747,2.11)],.009)
BODY.line('HoodEdge','silver_shaded',[(-.81,.995,.77),(0,1.03,.77),(.81,.995,.77)],.009)

# Hood-front lights and grilles on a nearly upright front fascia.
BODY.box('FrontNose','silver_shaded',(0,.58,2.24),(1.50,.19,.08))
for sign in (-1,1):
    BODY.box('HeadlightHousing','dark_metal',(sign*.54,.660,2.286),(.47,.095,.014))
    BODY.box('Headlamps','lamp_front',(sign*.55,.671,2.299),(.36,.052,.016))
    BODY.box('FrontIndicators','amber',(sign*.78,.60,2.281),(.12,.045,.018))
BODY.box('FrontGrille','black_line',(0,.596,2.294),(.48,.035,.015))
BODY.box('FrontBumper','dark_metal',(0,.47,2.286),(1.64,.055,.08))
BODY.box('RearBumper','dark_metal',(0,.47,-2.27),(1.70,.057,.085))
for sign in (-1,1):
    BODY.box('TailLight','lamp_rear',(sign*.63,.75,-2.283),(.38,.10,.026))
    BODY.box('RearIndicators','amber',(sign*.38,.75,-2.293),(.10,.082,.027))
BODY.box('RearGrille','glass_trim',(0,.731,-2.285),(.40,.11,.033))

# Passenger cabin: slanted windshield, roof, and rear sloped glass.
# Bottom and top of windshield
fbl=(-.805,1.028,.765); fbr=(.805,1.028,.765)
ftl=(-.655,1.465,.213); ftr=(.655,1.465,.213)
BODY.poly('Windscreen','glass_trim',[fbl,fbr,ftr,ftl],(0,.3,1))
BODY.poly('Windscreen','window_dark',[
    (-.739,1.042,.729),(.739,1.042,.729),(.605,1.449,.234),(-.605,1.449,.234)],(0,.4,1))
BODY.poly('Windscreen','window_reflection',[
    (-.70,1.056,.703),(-.39,1.056,.703),(-.50,1.429,.246),(-.58,1.429,.246)],(0,.4,1))
# roof two-part silver rectangular skin, side gutters.
# Narrow fixed center roof beam; the two outer roof wings move with the doors.
BODY.poly('FixedRoofBeam','silver_body',[
    (-.325,1.478,.231),(.325,1.478,.231),(.325,1.480,-.74),(-.325,1.480,-.74)],(0,1,0))
BODY.box('FixedRoofSpine','brushed_metal',(0,1.485,-.245),(.10,.023,.965))
for side in (-1,1):
    DOORS[side].poly('GullwingRoofPanel','silver_light',[
        (side*.325,1.480,.230),(side*.665,1.464,.230),
        (side*.690,1.472,-.740),(side*.325,1.480,-.740)],(0,1,0))
    DOORS[side].line('RoofSeam','brushed_metal',[
        (side*.325,1.485,.230),(side*.325,1.485,-.74)],.009)
# Simple low-poly interior becomes visible when doors open.
BODY.box('CabinFloor','dark_metal',(0,.547,-.04),(1.43,.045,1.48))
BODY.box('Dashboard','glass_trim',(0,.866,.54),(1.42,.13,.19))
BODY.box('DashboardDetail','black_line',(0,.951,.56),(1.21,.018,.15))
BODY.box('CenterConsole','glass_trim',(0,.682,-.19),(.29,.26,.91))
for side in (-1,1):
    BODY.box('SeatBase','window_dark',(side*.385,.635,-.33),(.49,.16,.55))
    BODY.box('SeatBack','window_dark',(side*.385,.918,-.59),(.49,.64,.13))
    BODY.box('SeatStripe','silver_shaded',(side*.385,.960,-.518),(.14,.40,.018))
for sign in [-1,1]:
    # side glass forward door section and rear quarter segment; gray glass is intentionally stylized.
    fwd=[(sign*.785,1.036,.672),(sign*.639,1.449,.182),
         (sign*.660,1.457,-.241),(sign*.808,1.03,-.144)]
    back=[(sign*.673,1.451,-.281),(sign*.684,1.451,-.690),
          (sign*.803,1.033,-1.080),(sign*.809,1.036,-.291)]
    DOORS[sign].poly('SideWindows','window_dark',fwd,(sign,0,0))
    BODY.poly('QuarterWindows','window_dark',back,(sign,0,0))
    # small glint toward the front top edge
    DOORS[sign].poly('GlassGlints','window_reflection',[
        (sign*.789,1.071,.581),(sign*.659,1.426,.170),
        (sign*.665,1.413,-.012),(sign*.790,1.085,.406)],(sign,0,0))
    DOORS[sign].line('WindowOutlines','glass_trim',[fwd[0],fwd[1],fwd[2],fwd[3],fwd[0]],.018)
    BODY.line('WindowOutlines','glass_trim',[back[0],back[1],back[2],back[3],back[0]],.018)
    DOORS[sign].line('DoorGutter','brushed_metal',[
        (sign*.674,1.48,.215),(sign*.681,1.482,-.27)],.018)
    BODY.line('RearRoofGutter','brushed_metal',[
        (sign*.688,1.482,-.285),(sign*.70,1.472,-.745)],.018)
    # door shape below window (gullwing inspired door lines)
    x=sign*.932
    DOORS[sign].line('DoorSeams','silver_shaded',[(x,.981,.62),(x+.008*sign,.82,.64),
            (x+.008*sign,.50,.57),(x+.008*sign,.48,-.52),
            (x+.008*sign,.89,-.52)],.009)
    DOORS[sign].box('DoorHandles','dark_metal',(sign*.945,.879,-.240),(.023,.025,.19))
    BODY.box('SideIntakes','dark_metal',(sign*.927,.70,-.92),(.018,.11,.27))
    for z in [-.96,-.89,-.82]:
        BODY.line('SideIntakeFins','silver_shaded',[
            (sign*.948,.67,z),(sign*.948,.735,z)],.007)
    DOORS[sign].box('DoorSideTrim','silver_shaded',(sign*.942,.560,.065),(.027,.043,1.18))
    BODY.box('RearSideTrim','silver_shaded',(sign*.942,.560,-.712),(.027,.043,.32))
    BODY.box('FrontSideTrim','silver_shaded',(sign*.942,.560,.75),(.027,.043,.15))
    # mirrors sit outboard near the windshield feet.
    DOORS[sign].tube('MirrorStalks','dark_metal',(sign*.80,1.059,.62),(sign*1.025,1.108,.72),.026)
    DOORS[sign].box('SideMirrors','silver_shaded',(sign*1.057,1.11,.74),(.18,.095,.20))
    DOORS[sign].box('MirrorGlass','glass_trim',(sign*1.132,1.11,.67),(.012,.066,.13))

# Rear window/vents and machine hump, molded like the photographed toy.
BODY.poly('BackGlass','window_dark',[
    (-.68,1.45,-.749),(.68,1.45,-.749),(.81,1.024,-1.221),(-.81,1.024,-1.221)],(0,0,-1))
for t in range(5):
    z=-.850-.074*t
    y=1.40-.071*t
    BODY.box('RearWindowLouvers','silver_shaded',(0,y,z),(1.46,.028,.05))
BODY.box('RearEngineDeck','silver_shaded',(0,1.016,-1.76),(1.52,.11,.90))
BODY.box('RearEngineFrame','dark_metal',(0,1.094,-1.67),(1.35,.018,.75))
for sign in (-1,1):
    # raised long machine conduits
    BODY.box('EngineDeckPanels','silver_body',(sign*.51,1.150,-1.72),(.46,.19,.73))
    BODY.box('MachineryHousings','brushed_metal',(sign*.57,1.245,-1.94),(.46,.10,.30))
    for zi in (-1.56,-1.68,-1.80,-1.92,-2.03):
        BODY.box('EngineVentSlots','dark_metal',(sign*.52,1.254,zi),(.32,.017,.020))
    # ribbed piping on either side of central electronics
    BODY.line('ReactorPipes','cable',[(sign*.77,1.10,-1.28),
       (sign*.78,1.25,-1.46),(sign*.77,1.32,-1.76),
       (sign*.75,1.23,-2.06)],.031,9)
    BODY.line('CableClips','brushed_metal',[(sign*.78,1.27,-1.66),
       (sign*.79,1.30,-1.72)],.017)
    BODY.box('RearSideModules','silver_shaded',(sign*.835,.99,-1.83),(.18,.15,.62))
# central device and cooling fins
BODY.box('CentralMachine','silver_body',(0,1.171,-1.76),(.46,.21,.63))
BODY.box('CentralDarkInset','dark_metal',(0,1.291,-1.68),(.30,.025,.34))
for z in np.linspace(-1.86,-1.51,5):
    BODY.box('CentralMachineRibs','silver_light',(0,1.312,float(z)),(.36,.029,.025))
for x in (-.15,.15):
    BODY.tube('RearCableRuns','cable',(x,1.31,-1.69),(x,1.29,-2.10),.024,9)
# Bumper reinforcements and small license plate (no text)
BODY.box('RearPlate','silver_light',(0,.67,-2.305),(.30,.105,.025))
BODY.box('RearPlateInset','dark_metal',(0,.67,-2.321),(.21,.062,.013))

# Build ONE wheel at origin, mesh geometry uses X as axle (Unity Y-up / Z-front).
WHEEL=Mesh()
segments=32
r=.342
axial_profile=[(-.114,.82),(-.114,.915),(-.100,1.0),(.100,1.0),(.114,.915),(.114,.82)]
for i in range(segments):
    a=2*math.pi*i/segments
    b=2*math.pi*(i+1)/segments
    for p in range(len(axial_profile)-1):
        x0,s0=axial_profile[p];x1,s1=axial_profile[p+1]
        v0=(x0,s0*r*math.sin(a),s0*r*math.cos(a))
        v1=(x0,s0*r*math.sin(b),s0*r*math.cos(b))
        v2=(x1,s1*r*math.sin(b),s1*r*math.cos(b))
        v3=(x1,s1*r*math.sin(a),s1*r*math.cos(a))
        material='rubber' if p!=2 else ('rubber_ridges' if i%4==0 else 'rubber')
        WHEEL.quad('Tire',material,v0,v1,v2,v3,
            (0,math.sin((a+b)*.5),math.cos((a+b)*.5)))
for side in (-1,1):
    sx=side*.116
    def cir(x,r,ang):return (x,r*math.sin(ang),r*math.cos(ang))
    for i in range(segments):
        a=2*math.pi*i/segments;b=2*math.pi*(i+1)/segments
        # rubber sidewall / rim ring / inner hub
        for ra,rb,mat in [(.81,.49,'rubber'),(.49,.43,'wheel_silver'),(.43,.20,'wheel_hub'),(.20,.07,'wheel_silver')]:
            WHEEL.quad('WheelRims',mat,cir(sx,ra*r,a),cir(sx,ra*r,b),cir(sx,rb*r,b),cir(sx,rb*r,a),(side,0,0))
        WHEEL.tri('WheelHubs','dark_metal',(sx+side*.009,0,0),cir(sx,.071*r,b),cir(sx,.071*r,a),(side,0,0))
    # 5 tiny raised center 'spokes' visible on both faces
    for j in range(5):
        angle=2*math.pi*j/5
        center=angle+math.pi/2
        dr=(0,math.sin(center),math.cos(center))
        dt=(0,math.cos(center),-math.sin(center))
        sx2=sx+side*.007
        quad=[]
        for axial,tang in [(0.08,-.026),(.34,-.045),(.34,.045),(.08,.026)]:
            quad.append((sx2,dr[1]*axial*r+dt[1]*tang,dr[2]*axial*r+dt[2]*tang))
        WHEEL.poly('WheelSpokes','wheel_silver',quad,(side,0,0))
# subtle tire tread lines to read well at game view
for i in range(0,segments,4):
    a=2*math.pi*(i+.5)/segments
    WHEEL.line('TreadGrooves','black_line',[(-.090,1.005*r*math.sin(a),1.005*r*math.cos(a)),
        (.090,1.005*r*math.sin(a),1.005*r*math.cos(a))],.005,4)

# export material library
with open(os.path.join(OUT,'ToyTimeMachine.mtl'),'w') as f:
    f.write('# MTL colors | uses no image textures\n')
    for name,(rgb,spec,ns) in MATS.items():
        f.write(f'newmtl {name}\nKa {rgb[0]*.35:.4f} {rgb[1]*.35:.4f} {rgb[2]*.35:.4f}\n')
        f.write(f'Kd {rgb[0]:.4f} {rgb[1]:.4f} {rgb[2]:.4f}\n')
        f.write(f'Ks {spec:.4f} {spec:.4f} {spec:.4f}\nNs {ns}\nd 1.0\nillum 2\n\n')

BODY.write(os.path.join(OUT,'CarBody.obj'))
WHEEL.write(os.path.join(OUT,'Wheel.obj'))
for side,name in [(-1,'DoorLeft'),(1,'DoorRight')]:
    pivot=np.asarray(DOOR_HINGES[side])
    local_mesh=Mesh()
    local_mesh.merge(DOORS[side],-pivot)
    local_mesh.write(os.path.join(OUT,name+'.obj'))
COMPLETE=Mesh()
COMPLETE.merge(BODY)
for side,name in [(-1,'L'),(1,'R')]:
    COMPLETE.merge(DOORS[side],name_prefix=('LeftDoor_' if side==-1 else 'RightDoor_'))
for side,name in [(-1,'L'),(1,'R')]:
    for z,axle in [(1.35,'F'),(-1.34,'R')]:
        COMPLETE.merge(WHEEL,(side*.937,.355,z),f'{name}{axle}_')
COMPLETE.write(os.path.join(OUT,'CarComplete_Preview.obj'))

# Dimension / mesh metadata for users and prefab generator.
metadata = {
  'model':'Photo-inspired retro time-machine toy coupe',
  'units':'meters','up_axis':'Y','forward_axis':'+Z',
  'approx_dimensions_m':[2.2,1.49,4.5],
  'wheel_radius_m':r,
  'doors': {'style':'gullwing','opening_axis':'local Z',
            'left_hinge_m':list(DOOR_HINGES[-1]),
            'right_hinge_m':list(DOOR_HINGES[1]),
            'left_open_rotation_degrees':-67,
            'right_open_rotation_degrees':67},
  'fire_effect':{'trigger_speed_mph':88,'trigger_speed_mps':88/2.2369362921,
                 'emitter_mount':'rear tire ground contact','particle_space':'world'},
  'wheel_centers_m':{
    'FL':[-.937,.355,1.35], 'FR':[.937,.355,1.35],
    'RL':[-.937,.355,-1.34], 'RR':[.937,.355,-1.34]
  },
  'source_note':'Estimated stylization from one user photo; hidden details approximated.'
}
with open(os.path.join(OUT,'mesh_info.json'),'w') as f:json.dump(metadata,f,indent=2)
print('Triangles body:',len(BODY.parts),'doors:',[len(DOORS[i].parts) for i in [-1,1]],'wheel:',len(WHEEL.parts),'combined:',len(COMPLETE.parts))
