import { Grid, makeStyles, Container, Typography } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  root: {
    padding: theme.spacing(3),
  },
}));

function Home() {
  const classes = useStyles();

  return (
    <Container maxWidth="sm" className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h2" gutterBottom>
            This is Admin Panel
          </Typography>
          <Typography variant="h4" gutterBottom>
            have a fun!
          </Typography>
        </Grid>
      </Grid>
    </Container>
  );
}

export default Home;
