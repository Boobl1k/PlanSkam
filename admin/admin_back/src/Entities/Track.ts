import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity()
export class Track{
    @PrimaryGeneratedColumn()
    Id: number;
    
    @Column()
    Name: string;
    
    @Column()
    AuthorId: number;
    
    @Column()
    TrackDataId: number;
}