import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity()
export class TrackData {
    @PrimaryGeneratedColumn()
    Id: number;
}